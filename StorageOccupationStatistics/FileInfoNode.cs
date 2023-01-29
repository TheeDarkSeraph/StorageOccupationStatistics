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
        public List<FileInfoNode> InnerFileNodes { get; private set; } = new();
        private readonly FileInfo _nodeFile;
        public FileInfoNode? ParentNode;
        private FileNodeUi _nodeUi;
        private long _fileSize;
        
        public FileInfoNode(FileInfo file) {
            ParentNode = null;
            _nodeFile = file;
            _nodeUi = new(this);
            if ((file.Attributes & FileAttributes.Directory) != 0) {
                SetupInnerFiles(file);
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
        public void SetupInnerFiles(FileInfo directory) {
            string[] innerFiles = GetFilesAndDirectories(directory);
            InnerFileNodes = new List<FileInfoNode>(innerFiles.Length);
            foreach (string fileName in innerFiles) {
                AddFileNode(InnerFileNodes, new FileInfo(fileName));
            }
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
            ParentNode = node;
        }

        public FileNodeUi? GetParentNodeUi() {
            return ParentNode?._nodeUi;
        }

        #region Getters

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
            foreach (FileInfoNode fileNode in InnerFileNodes) {
                sum += fileNode.GetAllFileCount(countFiles, countFolders);
            }
            return sum;
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

        public bool HasSubfiles() {
            return InnerFileNodes.Count > 0;
        }


        public float GetSizeRatio() {
            if (ParentNode == null){
                string rootPath = Path.GetPathRoot(GetFilePath());
                if (rootPath != null) {
                    long freeSpace = GetTotalFreeSpace(rootPath);
                    long totalSpace = GetTotalSpace(rootPath);
                    long takenSpace = totalSpace - freeSpace;
                    if (rootPath == GetFilePath()) {
                        return (float)(takenSpace / ((double)totalSpace));
                    } else {
                        return (float)(GetFileSize() / (double)takenSpace);
                    }
                } else {
                    return 1;
                }
            } else {
                long parentSize = Math.Max(ParentNode.GetFileSize(), 1);
                return (float)(GetFileSize() / ((double)parentSize));
            }
        }

        public long GetFileSize() {
            return _fileSize;
        }

        public void CalculateSize() {
            _fileSize = (_nodeFile.Attributes & FileAttributes.Directory) != 0 ? GetSumOfChildFilesSize() :
                // Not a directory
                _nodeFile.Length;
        }
        private long GetSumOfChildFilesSize() {
            return InnerFileNodes.Sum(fileNode => fileNode.GetFileSize());
        }
        #endregion



        public void PrintInnerInfo() {
            long totalSize = 0, totalFiles = 0, totalFolders = 0;
            foreach (FileInfoNode fileNode in InnerFileNodes) {
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

        public int GetIndexInParent() {
            return ParentNode!.InnerFileNodes.IndexOf(this);
        }

        public void DrawUi(Panel panel, int startXIndent, ref int startPrev) {
            _nodeUi.DrawSelf(panel, startXIndent, ref startPrev);
        }

        public void ReadjustUi(Panel panel, int indentLevel, ref int previousY) {
            _nodeUi.ReadjustUiPositions(panel, indentLevel, ref previousY);
        }
        public void ClearSelfAndChildrenUi(Panel panel) {
            _nodeUi.ClearUi(panel);
            ClearChildrenUi(panel);
        }

        public void ClearChildrenUi(Panel panel) {
            foreach (FileInfoNode nodeInfo in InnerFileNodes) {
                nodeInfo.ClearSelfAndChildrenUi(panel);
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
