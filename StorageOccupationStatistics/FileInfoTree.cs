using System.Diagnostics;
using System.Windows.Forms;

namespace StorageOccupationStatistics {

    public class FileInfoTree {
        //public static FileInfoTree instance;
        // I'll make this half singleton to avoid using too much memory.
        // there will usually only be one of this in the code
        private readonly FileInfoNode _rootNode;
        private Panel _myPanel;
        //public static List<Control> addedUI = new List<Control>();
        
        public FileInfoTree(string rootPath, Panel panel) {
            //if(instance != null)
            // clear the tree (make a function to clear all lists and lists inside
            // this will require to be implemented in the node too.
            //instance = this;
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
            _rootNode.DrawSelf(_myPanel, 0, ref prev);
            DrawingControl.ResumeDrawing(_myPanel);
            _myPanel.ResumeLayout();
        }

        public void ReDrawUi() {
            int prev = 4;
            _myPanel.SuspendLayout();
            DrawingControl.SuspendDrawing(_myPanel);
            _rootNode.ClearSelfAndChildrenUi(_myPanel);
            _rootNode.DrawSelf(_myPanel, 0, ref prev);
            DrawingControl.ResumeDrawing(_myPanel);
            _myPanel.ResumeLayout();
        }
        //private void ClearUI() {
        //    foreach (Control uiUnit in addedUI) {
        //        myPanel.Controls.Remove(uiUnit);
        //    }
        //    Debug.WriteLine(addedUI.Count);
        //    addedUI.Clear();
        //}
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
