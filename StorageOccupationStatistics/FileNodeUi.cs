using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StorageOccupationStatistics {
    public class FileNodeUi {
        private readonly FileInfoNode _fileInfoNode;
        
        private int _myIndent;
        public static readonly int UiHeight = 20, UiIndent = 16, BtnWidth = 20, ProgressInterX = 4,
            ProgressLabelWidth = 44, TotalUiWidth = 400, InterYSpace = 4, StartX = 4,
            ProgressLabelHeightAdj = 6;
        private bool _isExpanded;

        private static readonly Image ExpandedImage, CollapsedImage;
        private readonly List<Control> _uiControls = new();
        static FileNodeUi() {
            ExpandedImage = HelperFunctions.ResizeImage(Resource1.tr_d, UiHeight - 2, UiHeight - 2);
            CollapsedImage = HelperFunctions.ResizeImage(Resource1.tr_r, UiHeight - 2, UiHeight - 2);
        }

        public FileNodeUi(FileInfoNode fin) {
            _fileInfoNode = fin;
        }


        public void ChangeCollapse() {
            Panel panel = (Panel)_uiControls[0].Parent;
            HelperFunctions.PausePanel(panel);
            int scrollValue = GetVerticalScrollValue(panel);
            _isExpanded = !_isExpanded;
            if (_isExpanded) {
                DrawUiAsExpanded(panel);
            } else {
                DrawUiAsCollapsed(panel);
            }
            ReturnPanelToSavedVerticalScrollValue(panel, scrollValue);
            HelperFunctions.ResumePanel(panel);
            panel.PerformLayout();
        }

        private static int GetVerticalScrollValue(Panel panel) {
            return panel.VerticalScroll.Value;
        }

        private static void ReturnPanelToSavedVerticalScrollValue(Panel panel, int p) {
            panel.VerticalScroll.Value = Math.Min(p, panel.VerticalScroll.Maximum);
        }


        


        private void DrawUiAsExpanded(Panel panel) {
            Rectangle rect = _uiControls[^1].Bounds; // anyone will work
            // for some reason, this works...
            int previousY = rect.Y + panel.VerticalScroll.Value;
            if (_fileInfoNode.HasSubfiles()) ((Button)_uiControls[0]).Image = ExpandedImage;
            ClearUi(panel);
            DrawSelf(panel, _myIndent, ref previousY);
            if (_fileInfoNode.ParentNode == null) return;
            previousY -= panel.VerticalScroll.Value;
            _fileInfoNode.GetParentNodeUi()?.UpdateNeighborPositions(panel, _fileInfoNode, previousY);
        }
        private void DrawUiAsCollapsed(Panel panel) {
            _fileInfoNode.ClearChildrenUi(panel); // here
            if (_fileInfoNode.HasSubfiles()) ((Button)_uiControls[0]).Image = CollapsedImage;
            if (_fileInfoNode.ParentNode == null) return;
            Rectangle rect = _uiControls[^1].Bounds; // anyone will work
            // for some reason, this is the one with the problem..?
            int previousY = rect.Y + UiHeight + InterYSpace;
            _fileInfoNode.GetParentNodeUi()?.UpdateNeighborPositions(panel, _fileInfoNode, previousY);
            //_parentNode
        }

        public void DrawSelf(Panel panel, int indentLevel, ref int previousY) {
            SetupUiControls(panel, indentLevel, ref previousY);
            AddControlsToPanel(panel);
            previousY += UiHeight + InterYSpace;
            if (!_isExpanded) return;
            foreach (FileInfoNode nodeInfo in _fileInfoNode.InnerFileNodes) 
                nodeInfo.DrawUi(panel, indentLevel + 1, ref previousY);
        }
        private void AddControlsToPanel(Panel panel) {
            foreach (Control uiUnit in _uiControls) 
                panel.Controls.Add(uiUnit);
        }

        public void ClearUi(Panel panel) {
            RemoveControlsFromPanel(panel);
            _uiControls.Clear();
        }

        private void RemoveControlsFromPanel(Panel panel) {
            foreach (Control uiUnit in _uiControls) 
                panel.Controls.Remove(uiUnit);
        }

        private void SetupUiControls(Panel panel, int indentLevel, ref int previousY) {
            _myIndent = indentLevel;
            previousY -= panel.VerticalScroll.Value;
            int uiXLocation = StartX + indentLevel * UiIndent;
            if (_fileInfoNode.HasSubfiles()) 
                AddCollapseButton(panel, previousY, uiXLocation);
            uiXLocation += BtnWidth;
            AddProgressLabel(panel, previousY, uiXLocation);
            uiXLocation += ProgressLabelWidth + ProgressInterX;
            AddFilenameLabel(previousY, uiXLocation);
        }



        private void AddCollapseButton(Panel panel, int previousY, int uiXLocation) {
            Button btn = CreateCollapseButton(panel, previousY, uiXLocation);
            _uiControls.Add(btn);
        }

        private Button CreateCollapseButton(Panel panel, int previousY, int uiXLocation) {
            Button btn = new() {
                BackColor = Color.Transparent,
                Bounds = new Rectangle(uiXLocation, previousY, BtnWidth, UiHeight),
                Image = (_isExpanded ? ExpandedImage : CollapsedImage),
                FlatStyle = FlatStyle.Flat
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseDownBackColor = btn.BackColor;
            btn.FlatAppearance.MouseOverBackColor = btn.BackColor;
            btn.Click += ChangeCollapseEvent;
            return btn;
        }

        private void ChangeCollapseEvent(object? sender, EventArgs e) {
            ChangeCollapse();
        }
        private void AddProgressLabel(Panel panel, int previousY, int uiXLocation) {
            int adjustBorderRadius = (int)((UiHeight-ProgressLabelHeightAdj) * 0.8f);

            RoundProgressLabel rpLabel = new RoundProgressLabel() {
                Bounds = new Rectangle(uiXLocation,
                    previousY + ProgressLabelHeightAdj/2,
                    ProgressLabelWidth, UiHeight - ProgressLabelHeightAdj),
                FillRatio = _fileInfoNode.GetSizeRatio(),
                BorderRadius = adjustBorderRadius,
                //BorderSize = 2
            };
            _uiControls.Add(rpLabel);
        }
        private void AddFilenameLabel(int previousY, int uiXLocation) {
            _uiControls.Add(new Label() {
                BackColor = Color.Bisque,
                Text = _fileInfoNode.GetFileName(),
                Bounds = new Rectangle(uiXLocation, previousY, TotalUiWidth - uiXLocation, UiHeight)
            });
        }
        private void UpdateNeighborPositions(Panel panel, FileInfoNode childStartNode, int previousY) {
            int startIndex = childStartNode.GetIndexInParent();
            List<FileInfoNode> nodes = _fileInfoNode.InnerFileNodes;
            for (int i = startIndex + 1; i < nodes.Count; i++) {
                nodes[i].ReadjustUi(panel, _myIndent + 1, ref previousY);
            }
            _fileInfoNode.GetParentNodeUi()?.UpdateNeighborPositions(panel, _fileInfoNode, previousY);
        }

        public void ReadjustUiPositions(Panel panel, int indentLevel, ref int previousY) {
            UpdateUi(indentLevel, ref previousY);
            previousY += UiHeight + InterYSpace;

            if (!_isExpanded) return;
            foreach (FileInfoNode nodeInfo in _fileInfoNode.InnerFileNodes) {
                nodeInfo.ReadjustUi(panel, indentLevel + 1, ref previousY);
            }
        }
        private void UpdateUi(int indentLevel, ref int previousY) {
            _myIndent = indentLevel;
            foreach (Control ctrl in _uiControls) {
                Rectangle rect = ctrl.Bounds;
                if (ctrl is RoundProgressLabel) {
                    rect.Y = previousY + ProgressLabelHeightAdj/2;
                } else {
                    rect.Y = previousY;
                }

                ctrl.Bounds = rect;
            }
        }
    }
}
