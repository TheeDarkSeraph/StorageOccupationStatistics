using System.Diagnostics;
using System.Windows.Forms;

namespace StorageOccupationStatistics {

    public class FileInfoTree {
        private readonly FileInfoNode _rootNode;
        private static int UiYPad=4;
        private Panel _myPanel;
        private Panel _chartPanel;
        private Label _highlightLabel;

        public void UpdateHighlightLabelLocation(FileNodeUi node) {
            int yLocation=node.GetNodeYLocation();
            //yLocation += HelperFunctions.VerticalScrollValueOf(_myPanel);
            SetHighlightLabelYLocation(yLocation);
        }

        private void SetHighlightLabelYLocation(int yLocation) {
            Rectangle bounds = _highlightLabel.Bounds;
            bounds.Y = yLocation;
            _highlightLabel.Bounds = bounds;
        }

        // TODO: How can I get the click on the whole panel, regardless of the
        //      inner control?
        // Does this click event work even if we select inner items
        //  if not, we can make every component send click to panel event
        //  We also need to separate locations of 4+(24) - value of vert scroll

        //public void SetLabelLocationToMe() {
        //}
        
        public FileInfoTree(string rootPath, Panel panel) {
            _rootNode = new FileInfoNode(new FileInfo(rootPath));
            _myPanel = panel;
            CreateHighlightLabel();
            //AddHighlightLabelToPanel();
            UpdateHighlightLabelLocation(_rootNode.UiNode());
            DrawUI();
        }

        private void AddHighlightLabelToPanel() {
            _myPanel.Controls.Add(_highlightLabel);
        }

        private void CreateHighlightLabel() {
            Rectangle rect = _myPanel.Bounds;
            _highlightLabel = new Label() {
                BackColor = Color.FromArgb(100, 100, 0, 20),
                Text = "",
                Bounds = new Rectangle(0, UiYPad, _myPanel.Width, FileNodeUi.UiHeight)
            };
        }

        public Panel GetPanel() {
            return _myPanel;
        }
        public void SetForm(Panel panel) {
            _myPanel = panel;
        }
        public void DrawUI() {
            int prev = UiYPad;
            _myPanel.SuspendLayout();
            DrawingControl.SuspendDrawing(_myPanel);
            _rootNode.DrawUi(_myPanel, 0, ref prev);
            DrawingControl.ResumeDrawing(_myPanel);
            _myPanel.ResumeLayout();
        }

        public void ReDrawUi() {
            int prev = UiYPad;
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
