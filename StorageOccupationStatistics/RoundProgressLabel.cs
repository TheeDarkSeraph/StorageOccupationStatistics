using System.ComponentModel;
using System.Drawing.Drawing2D;
using FlatStyle = System.Windows.Forms.FlatStyle;

namespace StorageOccupationStatistics {
    // Lerp value for the colors (use starting and ending color

    public class RoundProgressLabel:Label {
        private Color _borderColor = Color.DarkBlue;
        private int _borderSize;
        private int _borderRadius = 40;
        private Color _startFillColor = Color.FromArgb(255, 0, 225, 0);
        private Color _middleColor = Color.FromArgb(255, 225, 225, 0);
        private Color _endFillColor = Color.FromArgb(255, 200, 0, 0);

        public float MinProgressValue { get; set; } = 0;
        public float MaxProgressValue { get; set; } = 100;
        public float ProgressValue { get; set; }

        [Category("zRoundedSettings")]
        public int BorderSize { 
            get =>_borderSize;
            set {
                _borderSize = value;
                Invalidate();
            }
        }

        [Category("zRoundedSettings")]
        public int BorderRadius {
            get {
                return _borderRadius;
            }
            set {
                _borderRadius = value;
                Invalidate();
            }
        }

        [Category("zRoundedSettings")]
        public Color BorderColor {
            get => _borderColor;
            set {
                _borderColor = value;
                Invalidate();
            }
        }

        [Category("zRoundedSettings")]
        public Color BackgroundColor {
            get => BackColor;
            set => BackColor = value;
        }

        [Category("zRoundedSettings")]
        public Color StartFillColor {
            get => _startFillColor;
            set {
                _startFillColor = value;
                Invalidate();
            }
        }
        [Category("zRoundedSettings")]
        public Color MiddleFillColor {
            get => _middleColor;
            set {
                _middleColor = value;
                Invalidate();
            }
        }
        [Category("zRoundedSettings")]
        public Color EndFillColor {
            get => _endFillColor;
            set {
                _endFillColor = value;
                Invalidate();
            }
        }

        [Category("zRoundedSettings")]
        public bool UseMiddleColor { get; set; } = true;

        [Category("zRoundedSettings")]
        public float FillRatio {
            get {
                if (ProgressValue > MaxProgressValue)
                    return 1;
                if (ProgressValue < MinProgressValue)
                    return 0;
                return (ProgressValue - MinProgressValue) / (MaxProgressValue - MinProgressValue);
            }
            set {
                ProgressValue = value switch {
                    > 1 => MaxProgressValue,
                    < 0 => MinProgressValue,
                    _ => MinProgressValue + value * (MaxProgressValue - MinProgressValue)
                };
                Invalidate();
            }
        }

        public RoundProgressLabel() {
            Text = "";
            AutoSize = false;
            FlatStyle = FlatStyle.Flat;
            Size = new(150, 80);
            BackColor = Color.LightSteelBlue;
            ForeColor = Color.Black;
        }
        
        private GraphicsPath GetFigurePath(RectangleF rect, float radius) {
            GraphicsPath path = new();

            //if (rect.Width < radius) {
            //    Debug.WriteLine("Condition 1");
            //} else if (rect.Width < radius*2) {
            //    Debug.WriteLine("Condition 2");

            //    rect.Height= rect.Height*MathF.Sin(rect.Width/radius);
            //}
            //if (radius > rect.Height)
            //    radius = rect.Height;
            if (radius == 0)
                return path;
            //Debug.WriteLine(rect.Height +" "+ rect.Width+" "+ radius+" TOTAL");
            path.StartFigure();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Width - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Width - radius, rect.Height-radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }

        public int GetLimitedRadius() {
            int limited = BorderRadius;
            if (limited > Height) {
                limited = Height;
            }
            if (limited > Width) {
                limited = Width;
            }
            return limited;
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            // we can clip using regions... interesting :D
            DrawBackgroundRegion();
            

            RectangleF rectSurface = new(0, 0, Width*FillRatio, Height);
            RectangleF rectBorder = new(0, 0, Width, Height);
            if (BorderRadius >= 1) { // Rounded Label
                
                Color brushColor;
                if (UseMiddleColor) {
                    brushColor = HelperFunctions.Lerp3Color(StartFillColor, MiddleFillColor, EndFillColor,
                        FillRatio, 0.1f, 0.9f);
                } else {
                    brushColor = HelperFunctions.LerpColor(StartFillColor, EndFillColor,
                        FillRatio, 0.15f, 0.9f);
                }
                using (GraphicsPath surfacePath = GetFigurePath(rectSurface, GetLimitedRadius()))
                using (GraphicsPath borderPath = GetFigurePath(rectBorder, GetLimitedRadius() - 1f))
                using (Brush surfaceBrush= new SolidBrush(brushColor))
                using (Pen borderPen = new(BorderColor, BorderSize)) {
                    e.Graphics.FillPath(surfaceBrush,surfacePath);
                    //e.Graphics.DrawPath(penSurface, pathSurface);
                    if (BorderSize >= 1) {
                        borderPen.Alignment = PenAlignment.Inset;
                        e.Graphics.DrawPath(borderPen, borderPath);
                    }
                }
            } else { // Normal Label

                if (BorderSize >= 1) {
                    using Pen borderPen = new(BorderColor, BorderSize);
                    borderPen.Alignment = PenAlignment.Inset;
                    e.Graphics.DrawRectangle(borderPen, 0, 0, Width, Height);
                }
            }
        }

        private void DrawBackgroundRegion() {
            RectangleF backgroundRect = new(0, 0, Width, Height);
            if (BorderIsRounded()) {
                using GraphicsPath backgroundPath = GetFigurePath(backgroundRect, GetLimitedRadius());
                using Pen backgroundPen = new(Parent.BackColor, 2);
                Region = new Region(backgroundPath);
            } else {
                Region = new Region(backgroundRect);
            }
        }

        private bool BorderIsRounded() {
            return BorderRadius >= 1;
        }

        protected override void OnHandleCreated(EventArgs e) {
            base.OnHandleCreated(e);
            Parent.BackColorChanged += Container_BackColorChanged;
        }

        private void Container_BackColorChanged(object? sender, EventArgs e) {
            if(DesignMode)
                Invalidate();
        }
    }
}
