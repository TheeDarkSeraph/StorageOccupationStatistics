// ctrl kd

//538794658702 501 // this number is theoritically wrong
//4854813


// C scan:
// 491 184 752 979
// 4358288 files, 213 285 folders
using System.Diagnostics;

namespace StorageOccupationStatistics {
    // TODO: associate file ratio of children with each other.
    // max file size = min (max,1)
    // Ratio = fileSize / max filsize  (to avoid zero division)
    public class FileInfoNode {
        public static readonly int uiHeight = 20, uiIndent = 16, btnWidth = 20,
            barLabelWidth = 40, totalUIWidth = 400, interYSpace = 4, startX = 4;
        private int myIndent = 0;
        List<FileInfoNode> innerFileNodes = new List<FileInfoNode>();
        FileInfo nodeFile;
        FileInfoNode? parentNode;
        public static Image expandedImage, collapsedImage;
        bool isExpanded = false;
        long fileSize;
        List<Control> nodeUI = new List<Control>();
        static FileInfoNode() {
            expandedImage = HelperFunctions.ResizeImage(Resource1.tr_d, uiHeight - 2, uiHeight - 2);
            collapsedImage = HelperFunctions.ResizeImage(Resource1.tr_r, uiHeight - 2, uiHeight - 2);
        }
        public FileInfoNode(FileInfo file) {
            parentNode = null;
            nodeFile = file;
            if ((file.Attributes & FileAttributes.Directory) != 0) {
                innerFileNodes = GetInnerFiles(file);
            }
            CalculateSize();
        }
        public string GetFileName() {
            string name = nodeFile.Name;
            if (name.Trim() == string.Empty) {
                return new DirectoryInfo(nodeFile.FullName).Name;
            } else {
                return nodeFile.Name;
            }
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
                //Debug.WriteLine("");
                return;
            }
            FileInfoNode node = new FileInfoNode(fileInfo);
            node.SetParentNode(this);
            nodeList.Add(node);

        }
        public void SetParentNode(FileInfoNode node) {
            parentNode = node;
        }

        private string[] GetFilesAndDirectories(FileInfo dirName) {
            string[] files = new string[0];
#pragma warning disable CS0168 // The variable 'e' is declared but never used
            try {
                files = Directory.GetFiles(dirName.FullName);
            } catch (Exception e) { // will not happen if authorized
                //Debug.WriteLine(e.StackTrace);
                Debug.WriteLine("Error auth with _ " + dirName.FullName);
            }
            string[] dirs = new string[0];
            try {
                dirs = Directory.GetDirectories(dirName.FullName);
            } catch (Exception e) { // will not happen if authorized
                //Debug.WriteLine(e.StackTrace);
            }
#pragma warning restore CS0168 // The variable 'e' is declared but never used
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
        public long GetFileSize() {
            return fileSize;
        }
        public void CalculateSize() {
            // the inner most nodes will get their size, and pass them upwards in the constructor
            // this will make each node calculated before hand and no need for duplicate
            // recursions happening

            if ((nodeFile.Attributes & FileAttributes.Directory) != 0) {
                if (innerFileNodes.Count == 0) { // I am an empty folder
                    fileSize = 0;
                } else {
                    long sum = 0;
                    foreach (FileInfoNode fileNode in innerFileNodes) {
                        sum += fileNode.GetFileSize();
                    }
                    fileSize = sum;
                }
            } else { // Not a directory
                fileSize = nodeFile.Length;
            }
        }
        public int GetAllFileCount(bool countFiles, bool countFolders) {
            if (nodeFile.Attributes.HasFlag(FileAttributes.System) && nodeFile.Attributes.HasFlag(FileAttributes.Hidden)) {
                //Debug.WriteLine("System "+nodeFile.FullName);
                return 0;
            }
            if ((nodeFile.Attributes & FileAttributes.Directory) != 0) {
                return (countFolders ? 1 : 0) + GetChildFoldersCount(countFiles, countFolders);
            } else { // Not a directory
                return (countFiles ? 1 : 0);
            }
        }
        public int GetAllFileCountExcludingSelf(bool countFiles, bool countFolders) {
            if ((nodeFile.Attributes & FileAttributes.Directory) != 0) {
                return GetChildFoldersCount(countFiles, countFolders);
            } else { // Not a directory
                return 0;
            }
        }
        public int GetChildFoldersCount(bool countFiles, bool countFolders) {
            int sum = 0;
            foreach (FileInfoNode fileNode in innerFileNodes) {
                sum += fileNode.GetAllFileCount(countFiles, countFolders);
            }
            return sum;
        }
        public void PrintInnerInfo() {
            long totalSize = 0, totalFiles = 0, totalFolders = 0;
            long t1, t2, t3;
            foreach (FileInfoNode fileNode in innerFileNodes) {
                Debug.WriteLine(fileNode.nodeFile.FullName);
                t1 = fileNode.GetFileSize();
                t2 = fileNode.GetAllFileCount(true, false);
                t3 = fileNode.GetAllFileCount(false, true);
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

        //public void Expand() {
        //    isExpanded = true;
        //}
        //public void ExpandChildren() {
        //    foreach (FileInfoNode nodeInfo in innerFileNodes) {
        //        nodeInfo.Expand();
        //    }
        //}
        //public void Collapse() {
        //    isExpanded = false;
        //}

        public void DrawSelf(Panel panel, int indentLevel, ref int previousY) {
            ReadyUI(panel, indentLevel, ref previousY);
            foreach (Control uiUnit in nodeUI) {
                panel.Controls.Add(uiUnit);
                FileInfoTree.addedUI.Add(uiUnit);
            }
            previousY += uiHeight + interYSpace;
            if (isExpanded) {
                foreach (FileInfoNode nodeInfo in innerFileNodes) {
                    nodeInfo.DrawSelf(panel, indentLevel + 1, ref previousY);
                }
            }
        }
        private void ReadyUI(Panel panel, int indentLevel, ref int previousY) {
            myIndent = indentLevel;
            int uiXLocation = startX + indentLevel * uiIndent;
            if (innerFileNodes.Count > 0) {
                Button btn = new Button() {
                    BackColor = Color.Transparent,
                    Bounds = new Rectangle(uiXLocation, previousY - panel.VerticalScroll.Value, btnWidth, uiHeight),
                    Image = (isExpanded ? expandedImage : collapsedImage)
                };
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseDownBackColor = btn.BackColor;
                btn.FlatAppearance.MouseOverBackColor = btn.BackColor;
                btn.Click += new EventHandler(ChangeCollapseEvent);
                nodeUI.Add(btn);
            }
            uiXLocation += btnWidth;
            //Image for image refs And on click listener
            nodeUI.Add(new Label() {
                BackColor = Color.Green,
                Bounds = new Rectangle(uiXLocation, previousY - panel.VerticalScroll.Value, barLabelWidth, uiHeight)
            });
            uiXLocation += barLabelWidth;
            nodeUI.Add(new Label() {
                BackColor = Color.Bisque,
                Text = GetFileName(),
                Bounds = new Rectangle(uiXLocation, previousY - panel.VerticalScroll.Value, totalUIWidth - uiXLocation, uiHeight)
            });
        }

        private void ChangeCollapseEvent(object? sender, EventArgs e) {
            ChangeCollapse();
        }
        public void ChangeCollapse() {
            // tabstop = true
            Panel panel = FileInfoTree.GetInstPanel();
            int p = panel.VerticalScroll.Value;
            //int sVal=panel.HorizontalScroll.Value;
            isExpanded = !isExpanded;
            if (isExpanded) {
                Rectangle rect = nodeUI[nodeUI.Count - 1].Bounds; // anyone will work
                
                // for some reason, this works...
                int previousY = rect.Y + panel.VerticalScroll.Value;
                if (innerFileNodes.Count > 0) ((Button)nodeUI[0]).Image = expandedImage;
                ClearMyUI(panel);
                Debug.WriteLine("my indent " + myIndent);
                DrawSelf(panel, myIndent, ref previousY);
                if (parentNode != null) {
                    previousY -= panel.VerticalScroll.Value;
                    parentNode.UpdateNeighborPositions(this, previousY);
                }
                // This needs to be of the LAST child
                //FileInfoTree.instance.ReDrawUI();
            } else {
                ClearChildrenUI(panel); // here
                if (innerFileNodes.Count > 0) ((Button)nodeUI[0]).Image = collapsedImage;
                if (parentNode != null) {
                    Rectangle rect = nodeUI[nodeUI.Count - 1].Bounds; // anyone will work
                    // for some reason, this is the one with the problem..?
                    int previousY = rect.Y + uiHeight + interYSpace;
                    parentNode.UpdateNeighborPositions(this, previousY);
                    //parentNode
                }
            }

            //nodeUI[0].Select();//  Select();
            panel.VerticalScroll.Value = Math.Min(p, panel.VerticalScroll.Maximum);
            panel.PerformLayout();
            //panel.HorizontalScroll.Value = sVal;
            // redraw UI, this will require us knowing the parent
        }
        public void ClearMyAndChildrenUI(Panel panel) {
            ClearMyUI(panel);
            ClearChildrenUI(panel);
        }
        private void ClearMyUI(Panel panel) {
            foreach (Control uiUnit in nodeUI) {
                panel.Controls.Remove(uiUnit);
                FileInfoTree.addedUI.Remove(uiUnit);
            }
            nodeUI.Clear();
        }
        private void ClearChildrenUI(Panel panel) {
            foreach (FileInfoNode nodeInfo in innerFileNodes) {
                nodeInfo.ClearMyAndChildrenUI(panel);
            }
        }
        private void UpdateNeighborPositions(FileInfoNode startNode, int previousY) {
            int startIndex = innerFileNodes.IndexOf(startNode);
            for (int i = startIndex + 1; i < innerFileNodes.Count; i++) {
                innerFileNodes[i].ReDrawSelf(FileInfoTree.GetInstPanel(), myIndent + 1, ref previousY);
            }
            if (parentNode != null) {
                parentNode.UpdateNeighborPositions(this, previousY);
            }
        }

        public void ReDrawSelf(Panel panel, int indentLevel, ref int previousY) {
            UpdateUI(panel, indentLevel, ref previousY);
            previousY += uiHeight + interYSpace;
            if (isExpanded) {
                foreach (FileInfoNode nodeInfo in innerFileNodes) {
                    nodeInfo.ReDrawSelf(panel, indentLevel + 1, ref previousY);
                }
            }
        }
        private void UpdateUI(Panel panel, int indentLevel, ref int previousY) {
            myIndent = indentLevel;
            foreach (Control ctrl in nodeUI) {
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
