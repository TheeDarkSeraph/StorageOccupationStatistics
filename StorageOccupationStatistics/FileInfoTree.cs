using System.Diagnostics;
using System.Windows.Forms;

namespace StorageOccupationStatistics {

    public class FileInfoTree {
        public static FileInfoTree instance;
        // I'll make this half singleton to avoid using too much memory.
        // there will usually only be one of this in the code
        FileInfoNode rootNode;
        Panel myPanel;
        public static List<Control> addedUI = new List<Control>();
        
        public FileInfoTree(string rootPath, Panel panel) {
            //if(instance != null)
            // clear the tree (make a function to clear all lists and lists inside
            // this will require to be implemented in the node too.
            instance = this;
            rootNode = new FileInfoNode(new FileInfo(rootPath));
            myPanel = panel;
            DrawUI();
        }
        public static Panel GetInstPanel() {
            return instance.myPanel;
        }
        public Panel GetPanel() {
            return myPanel;
        }
        public void SetForm(Panel panel) {
            myPanel = panel;
        }
        public void DrawUI() {
            int prev = 4;
            rootNode.DrawSelf(myPanel, 0, ref prev);
        }

        public void ReDrawUI() {
            int prev = 4;
            rootNode.ClearMyAndChildrenUI(myPanel);
            rootNode.DrawSelf(myPanel, 0, ref prev);

        }
        //private void ClearUI() {
        //    foreach (Control uiUnit in addedUI) {
        //        myPanel.Controls.Remove(uiUnit);
        //    }
        //    Debug.WriteLine(addedUI.Count);
        //    addedUI.Clear();
        //}
        public FileInfoNode GetRoot() {
            return rootNode;
        }
        public long GetTotalSize() {
            return rootNode.GetFileSize();
        }
        public int GetFileCount() {
            return rootNode.GetAllFileCountExcludingSelf(true, false);
        }
        public int GetFolderCount() {
            return rootNode.GetAllFileCountExcludingSelf(false, true);
        }
        public void PrintRootOneLayerInfo() {
            rootNode.PrintInnerInfo();
        }

    }
}
