// ctrl kd

//538794658702 501 // this number is theoritically wrong
//4854813


// C scan:
// 491 184 752 979
// 4358288 files, 213 285 folders
using System.Diagnostics;

namespace StorageOccupationStatistics {
    // Done: fix the instant refresh after adding a new control
    // IMPORTANT ^^^^^^^^^^^^^^^^^^^^^^^^

    // Done: Find another solution other than using the global instance variable for the tree (maybe just assign it to some kind of variable).

    // TODO: Add a choose your path
    // TODO: Add a loading screen or something...?

    // Done: before we make the ratio of the children and such, we need to apply the progress UI first
    //          So we figure out how to make this progress UI then we figure out how to fill it.

    // TODO: associate the parent node ratio to be relative to drive's taken size

    // TODO: selection highlight of whole row (is it a label highlight? or what should it be?)
    //          Can a label pass through commands to things under it like the expansion button?

    // Done: associate file ratio of children with each other.
    // max file size = min (max,1)
    // Ratio = fileSize / max filsize  (to avoid zero division)
    // TODO: Can we make this code cleaner?

    public class FileInfoNode {
        public static readonly int UiHeight = 20, UiIndent = 16, BtnWidth = 20, ProgressInterX=4,
            ProgressLabelWidth = 44, TotalUiWidth = 400, InterYSpace = 4, StartX = 4;
        private int _myIndent;
        private readonly List<FileInfoNode> _innerFileNodes = new();
        private readonly FileInfo _nodeFile;
        private FileInfoNode? _parentNode;
        private static readonly Image ExpandedImage, CollapsedImage;
        private bool _isExpanded;
        private long _fileSize;
        private readonly List<Control> _nodeUi = new ();
        static FileInfoNode() {
            ExpandedImage = HelperFunctions.ResizeImage(Resource1.tr_d, UiHeight - 2, UiHeight - 2);
            CollapsedImage = HelperFunctions.ResizeImage(Resource1.tr_r, UiHeight - 2, UiHeight - 2);
        }
        public FileInfoNode(FileInfo file) {
            _parentNode = null;
            _nodeFile = file;
            if ((file.Attributes & FileAttributes.Directory) != 0) {
                _innerFileNodes = GetInnerFiles(file);
            }
            CalculateSize();
        }
        public string GetFileName()
        {
            string name = _nodeFile.Name;
            return name.Trim() == string.Empty ? new DirectoryInfo(_nodeFile.FullName).Name : _nodeFile.Name;
        }

        public string? GetFilePath() {
            return _nodeFile.FullName;
        }
        public List<FileInfoNode> GetInnerFiles(FileInfo directory) {
            string[] innerFiles = GetFilesAndDirectories(directory);
            List<FileInfoNode> innerNodes = new List<FileInfoNode>(innerFiles.Length);
            foreach (string fileName in innerFiles) {
                AddFileNode(innerNodes, new FileInfo(fileName));
            }
            return innerNodes;
        }
        private void AddFileNode(List<FileInfoNode> nodeList, FileInfo fileInfo) {
            if (fileInfo.Attributes.HasFlag(FileAttributes.ReparsePoint)) {
                return;
            }
            FileInfoNode node = new(fileInfo);
            node.SetParentNode(this);
            nodeList.Add(node);

        }
        public void SetParentNode(FileInfoNode node) {
            _parentNode = node;
        }

#pragma warning disable CS0168 // The variable 'e' is declared but never used
        private string[] GetFilesAndDirectories(FileInfo dirName) {
            string[] files = Array.Empty<string>();
            string[] dirs = files;
            try {
                files = Directory.GetFiles(dirName.FullName);
            } catch (Exception) {
                // ignored
            }
            try {
                dirs = Directory.GetDirectories(dirName.FullName);
            } catch (Exception) {
                // ignored
            }
            //Environment.Exit(0);
            string[] allEntries = new string[files.Length + dirs.Length];
            for (int i = 0; i < files.Length; i++) {
                allEntries[i] = files[i];
            }
            for (int i = 0; i < dirs.Length; i++) {
                allEntries[i + files.Length] = dirs[i];
            }
            return allEntries;
        }
#pragma warning restore CS0168 // The variable 'e' is declared but never used

        public int GetAllFileCount(bool countFiles, bool countFolders) {
            if (_nodeFile.Attributes.HasFlag(FileAttributes.System) && _nodeFile.Attributes.HasFlag(FileAttributes.Hidden)) {
                //Debug.WriteLine("System "+_nodeFile.FullName);
                return 0;
            }
            if ((_nodeFile.Attributes & FileAttributes.Directory) != 0) {
                return (countFolders ? 1 : 0) + GetChildFoldersCount(countFiles, countFolders);
            } else { // Not a directory
                return (countFiles ? 1 : 0);
            }
        }
        public int GetAllFileCountExcludingSelf(bool countFiles, bool countFolders) {
            if ((_nodeFile.Attributes & FileAttributes.Directory) != 0) {
                return GetChildFoldersCount(countFiles, countFolders);
            } else { // Not a directory
                return 0;
            }
        }
        public int GetChildFoldersCount(bool countFiles, bool countFolders) {
            int sum = 0;
            foreach (FileInfoNode fileNode in _innerFileNodes) {
                sum += fileNode.GetAllFileCount(countFiles, countFolders);
            }
            return sum;
        }

#region Add UI Components
        private void AddUiComponents(Panel panel, int indentLevel, ref int previousY) {
            _myIndent = indentLevel;
            int uiXLocation = StartX + indentLevel * UiIndent;
            if (_innerFileNodes.Count > 0) {
                AddCollapseButton(panel, previousY, uiXLocation);
            }
            uiXLocation += BtnWidth;
            //Image for image refs And on click listener
            AddProgressLabel(panel, previousY, uiXLocation);
            uiXLocation += ProgressLabelWidth+ProgressInterX;
            _nodeUi.Add(new Label() {
                BackColor = Color.Bisque,
                Text = GetFileName(),
                Bounds = new Rectangle(uiXLocation, previousY - panel.VerticalScroll.Value, TotalUiWidth - uiXLocation, UiHeight)
            });
        }


        private void AddCollapseButton(Panel panel, int previousY, int uiXLocation) {
            Button btn = CreateCollapseButton(panel, previousY, uiXLocation);
            _nodeUi.Add(btn);
        }

        private Button CreateCollapseButton(Panel panel, int previousY, int uiXLocation) {
            Button btn = new() {
                BackColor = Color.Transparent,
                Bounds = new Rectangle(uiXLocation, previousY - panel.VerticalScroll.Value, BtnWidth, UiHeight),
                Image = (_isExpanded ? ExpandedImage : CollapsedImage),
                FlatStyle = FlatStyle.Flat
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseDownBackColor = btn.BackColor;
            btn.FlatAppearance.MouseOverBackColor = btn.BackColor;
            btn.Click += ChangeCollapseEvent;
            return btn;
        }

        private void AddProgressLabel(Panel panel, int previousY, int uiXLocation) {
            int adjustBorderRadius = (int)(UiHeight * 0.5f);
            int heightAdjustment = 6;
            RoundProgressLabel rpLabel = new RoundProgressLabel() {
                Bounds = new Rectangle(uiXLocation, 
                    previousY - panel.VerticalScroll.Value + heightAdjustment / 2,
                    ProgressLabelWidth, UiHeight - heightAdjustment),
                FillRatio = GetSizeRatio(),
                BorderRadius = adjustBorderRadius,
                //BorderSize = 2
            };
            _nodeUi.Add(rpLabel);
        }
        private long GetTotalFreeSpace(string driveName) {
            foreach (DriveInfo drive in DriveInfo.GetDrives()) {
                if (drive.IsReady && drive.Name == driveName) {
                    return drive.TotalFreeSpace; // avail takes into account quota's for each account
                }
            }
            return -1;
        }
        private long GetTotalSpace(string driveName) {
            foreach (DriveInfo drive in DriveInfo.GetDrives()) {
                if (drive.IsReady && drive.Name == driveName) {
                    return drive.TotalSize; // avail takes into account quota's for each account
                }
            }
            return -1;
        }

        private float GetSizeRatio() {
            if (_parentNode == null){

                Debug.WriteLine("is root? " + GetFilePath());
                Debug.WriteLine("is root2? " + Path.GetPathRoot(GetFilePath()));

                string rootPath = Path.GetPathRoot(GetFilePath());
                if (rootPath != null) {
                    long freeSpace = GetTotalFreeSpace(rootPath);
                    long totalSpace = GetTotalSpace(rootPath);
                    long takenSpace = totalSpace - freeSpace;
                    if (rootPath == GetFilePath()) {
                        Debug.WriteLine("Root satisfied");
                        return (float)(takenSpace / ((double)totalSpace));
                    } else {
                        return (float)(GetFileSize() / (double)takenSpace);
                    }
                } else {
                    return 1;
                }
            } else {
                long parentSize = Math.Max(_parentNode.GetFileSize(), 1);
                return (float)(GetFileSize() / ((double)parentSize));
            }
        }

        public long GetFileSize() {
            return _fileSize;
        }

        public void CalculateSize() {
            // the inner most nodes will get their size, and pass them upwards in the constructor
            // this will make each node calculated before hand and no need for duplicate
            // recursions happening

            _fileSize = (_nodeFile.Attributes & FileAttributes.Directory) != 0 ? GetSumOfChildFilesSize() :
                // Not a directory
                _nodeFile.Length;
        }
        private long GetSumOfChildFilesSize() {
            return _innerFileNodes.Sum(fileNode => fileNode.GetFileSize());
        }
#endregion



        public void PrintInnerInfo() {
            long totalSize = 0, totalFiles = 0, totalFolders = 0;
            foreach (FileInfoNode fileNode in _innerFileNodes) {
                Debug.WriteLine(fileNode._nodeFile.FullName);
                long t1 = fileNode.GetFileSize();
                long t2 = fileNode.GetAllFileCount(true, false);
                long t3 = fileNode.GetAllFileCount(false, true);
                Debug.WriteLine(t1 + " " + (t1 / (1024 * 1024 * 1024)));
                Debug.WriteLine("Files : " + t2);
                Debug.WriteLine("Folders : " + t3);
                totalSize += t1;
                totalFiles += t2;
                totalFolders += t3;
            }
            Debug.WriteLine("All:");
            Debug.WriteLine(totalSize);
            Debug.WriteLine("Files : " + totalFiles);
            Debug.WriteLine("Folders : " + totalFolders);
        }

        private void ChangeCollapseEvent(object? sender, EventArgs e) {
            ChangeCollapse();
        }
        public void ChangeCollapse() {
            // tabstop = true
            Panel panel = (Panel)_nodeUi[0].Parent;
            panel.SuspendLayout();
            DrawingControl.SuspendDrawing(panel);
            //panel.SuspendLayout();
            int p = panel.VerticalScroll.Value;
            _isExpanded = !_isExpanded;
            if (_isExpanded) {
                Rectangle rect = _nodeUi[^1].Bounds; // anyone will work
                
                // for some reason, this works...
                int previousY = rect.Y + panel.VerticalScroll.Value;
                if (_innerFileNodes.Count > 0) ((Button)_nodeUi[0]).Image = ExpandedImage;
                ClearMyUi(panel);
                //Debug.WriteLine("my indent " + _myIndent);
                DrawSelf(panel, _myIndent, ref previousY);
                if (_parentNode != null) {
                    previousY -= panel.VerticalScroll.Value;
                    _parentNode.UpdateNeighborPositions(panel,this, previousY);
                }
            } else {
                ClearChildrenUi(panel); // here
                if (_innerFileNodes.Count > 0) ((Button)_nodeUi[0]).Image = CollapsedImage;
                if (_parentNode != null) {
                    Rectangle rect = _nodeUi[^1].Bounds; // anyone will work
                    // for some reason, this is the one with the problem..?
                    int previousY = rect.Y + UiHeight + InterYSpace;
                    _parentNode.UpdateNeighborPositions(panel,this, previousY);
                    //_parentNode
                }
            }

            //_nodeUi[0].Select();//  Select();
            panel.VerticalScroll.Value = Math.Min(p, panel.VerticalScroll.Maximum);
            DrawingControl.ResumeDrawing(panel);
            panel.ResumeLayout();

            panel.PerformLayout();
            // redraw UI, this will require us knowing the parent
        }

        //public void DrawSelfWithSuspend(Panel panel, int indentLevel, ref int previousY) {
        //    panel.SuspendLayout();
        //    DrawSelf(panel, indentLevel, ref previousY);
        //    panel.ResumeLayout();
        //}
        public void DrawSelf(Panel panel, int indentLevel, ref int previousY) {
            AddUiComponents(panel, indentLevel, ref previousY);
            foreach (Control uiUnit in _nodeUi) {
                panel.Controls.Add(uiUnit);
                //FileInfoTree.addedUI.Add(uiUnit);
            }
            previousY += UiHeight + InterYSpace;
            if (!_isExpanded) return;
            foreach (FileInfoNode nodeInfo in _innerFileNodes) {
                nodeInfo.DrawSelf(panel, indentLevel + 1, ref previousY);
            }
        }
        public void ClearSelfAndChildrenUi(Panel panel) {
            ClearMyUi(panel);
            ClearChildrenUi(panel);
        }
        private void ClearMyUi(Panel panel) {
            foreach (Control uiUnit in _nodeUi) {
                panel.Controls.Remove(uiUnit);
                //FileInfoTree.addedUI.Remove(uiUnit);
            }
            _nodeUi.Clear();
        }
        private void ClearChildrenUi(Panel panel) {
            foreach (FileInfoNode nodeInfo in _innerFileNodes) {
                nodeInfo.ClearSelfAndChildrenUi(panel);
            }
        }
        // the caller of this should suspend
        private void UpdateNeighborPositions(Panel panel, FileInfoNode startNode, int previousY) {
            int startIndex = _innerFileNodes.IndexOf(startNode);
            for (int i = startIndex + 1; i < _innerFileNodes.Count; i++) {
                _innerFileNodes[i].ReadjustSelf(panel, _myIndent + 1, ref previousY);
            }
            _parentNode?.UpdateNeighborPositions(panel, this, previousY);
        }

        public void ReadjustSelf(Panel panel, int indentLevel, ref int previousY) {
            UpdateUi(indentLevel, ref previousY);
            previousY += UiHeight + InterYSpace;

            if (!_isExpanded) return;
            foreach (FileInfoNode nodeInfo in _innerFileNodes) {
                nodeInfo.ReadjustSelf(panel, indentLevel + 1, ref previousY);
            }
        }
        private void UpdateUi(int indentLevel, ref int previousY) {
            _myIndent = indentLevel;
            foreach (Control ctrl in _nodeUi) {
                Rectangle rect = ctrl.Bounds;
                rect.Y = previousY;
                ctrl.Bounds = rect;
            }
        }

        // I think this is a failure
        // pass a path, the tree should call this somehow
        public void SaveInFile() {

        }
        public void LoadFromFile() {

        }

    }
}
