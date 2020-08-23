using System.Drawing;

namespace SmartPixel.Extensions
{
    // Converts System.Windows.Media.Color to System.Drawing.Color
    public static class ConvertColorType
    {
        public static Color ConvertToDrawing(this System.Windows.Media.Color winColor)
        {
            return Color.FromArgb(winColor.A, winColor.R, winColor.G, winColor.B);
        }

        public static System.Windows.Media.Color ConvertToMedia(this System.Drawing.Color winColor)
        {
            return System.Windows.Media.Color.FromArgb(winColor.A, winColor.R, winColor.G, winColor.B);
        }
    }
}
