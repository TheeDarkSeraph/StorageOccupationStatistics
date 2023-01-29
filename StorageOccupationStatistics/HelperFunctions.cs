using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageOccupationStatistics {
    public class HelperFunctions {

        public static void PausePanel(Panel panel) {
            panel.SuspendLayout();
            DrawingControl.SuspendDrawing(panel);
        }
        public static void ResumePanel(Panel panel) {
            DrawingControl.ResumeDrawing(panel);
            panel.ResumeLayout();
        }
        public static Color LerpColor(Color start, Color end, float lerpValue, float lerpStart, float lerpEnd) {
            if (lerpValue > lerpEnd)
                return end;
            if (lerpValue < lerpStart)
                return start;
            float lerpRatio = (lerpValue - lerpStart) / (lerpEnd - lerpStart);
            return Color.FromArgb(
                (int)(start.A + (end.A - start.A) * lerpRatio + 0.5f),
                (int)(start.R + (end.R - start.R) * lerpRatio + 0.5f),
                (int)(start.G + (end.G - start.G) * lerpRatio + 0.5f),
                (int)(start.B + (end.B - start.B) * lerpRatio + 0.5f)
            );
        }

        public static Color Lerp3Color(Color start, Color middle, Color end, float lerpValue, float lerpStart, float lerpEnd) {
            if (lerpValue > lerpEnd)
                return end;
            if (lerpValue < lerpStart)
                return start;
            float lerpRatio = (lerpValue - lerpStart) / (lerpEnd - lerpStart);
            if (lerpRatio < 0.5) {
                lerpRatio *= 2; //range becomes 0 to 0.5
                end = middle;
            } else {
                start = middle;
                lerpRatio -= 0.5f;
                lerpRatio *= 2;

            }
            
            return Color.FromArgb(
                (int)(start.A + (end.A - start.A) * lerpRatio + 0.5f),
                (int)(start.R + (end.R - start.R) * lerpRatio + 0.5f),
                (int)(start.G + (end.G - start.G) * lerpRatio + 0.5f),
                (int)(start.B + (end.B - start.B) * lerpRatio + 0.5f)
            );
        }
        public static Bitmap ResizeImage(Image image, int width, int height) {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage)) {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes()) {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}
