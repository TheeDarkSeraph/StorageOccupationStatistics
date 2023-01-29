using System.Diagnostics;
using System.Windows.Forms;

namespace StorageOccupationStatistics {

    public class FileInfoTree {
        private readonly FileInfoNode _rootNode;
        private Panel _myPanel;
        
        public FileInfoTree(string rootPath, Panel panel) {
            _rootNode = new FileInfoNode(new FileInfo(rootPath));
            _myPanel = panel;
            DrawUI();
        }
        public Panel GetPanel() {
            return _myPanel;
        }
        public void SetForm(Panel panel) {
            _myPanel = panel;
        }
        public void DrawUI() {
            int prev = 4;
            _myPanel.SuspendLayout();
            DrawingControl.SuspendDrawing(_myPanel);
            _rootNode.DrawUi(_myPanel, 0, ref prev);
            DrawingControl.ResumeDrawing(_myPanel);
            _myPanel.ResumeLayout();
        }

        public void ReDrawUi() {
            int prev = 4;
            HelperFunctions.PausePanel(_myPanel);
            _rootNode.ClearSelfAndChildrenUi(_myPanel);
            _rootNode.DrawUi(_myPanel, 0, ref prev);
            HelperFunctions.ResumePanel(_myPanel);
        }
        public FileInfoNode GetRoot() {
            return _rootNode;
        }
        public long GetTotalSize() {
            return _rootNode.GetFileSize();
        }
        public int GetFileCount() {
            return _rootNode.GetAllFileCountExcludingSelf(true, false);
        }
        public int GetFolderCount() {
            return _rootNode.GetAllFileCountExcludingSelf(false, true);
        }
        public void PrintRootOneLayerInfo() {
            _rootNode.PrintInnerInfo();
        }

    }
}
