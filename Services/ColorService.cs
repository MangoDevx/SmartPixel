using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SmartPixel.Services
{
    public class ColorService
    {
        public Color GetNextColor()
        {
            var random = new Random();
            var colorBytes = new byte[3];
            // Selects a color value with byte 0 - 155 then adds 100 to it. Min Value: 100 Max Value: 255
            colorBytes[0] = (byte)(random.Next(156) + 100);
            colorBytes[1] = (byte)(random.Next(156) + 100);
            colorBytes[2] = (byte)(random.Next(156) + 100);

            return Color.FromArgb(colorBytes[0], colorBytes[1], colorBytes[2]);
        }

        public Color GetNextGrayColor()
        {
            // Selects a color value with byte 0 - 155 then adds 100 to it. Min Value: 100 Max Value: 255
            var random = new Random();
            var color = (byte)(random.Next(156) + 100);

            return Color.FromArgb(color, color, color);
        }

        // Stack overflow OP. Modified distance formula for the RGB of the selected color vs target color
        public Color SelectClosestColor(List<Color> colors, Color targetColor)
        {
            var colorDiffs = colors.Select(n => ColorDiff(n, targetColor)).Min(n => n);
            return colors[colors.FindIndex(n => ColorDiff(n, targetColor) == colorDiffs)];
        }

        private int ColorDiff(Color c1, Color c2)
        {
            return (int)Math.Sqrt((c1.R - c2.R) * (c1.R - c2.R)
                                     + (c1.G - c2.G) * (c1.G - c2.G)
                                     + (c1.B - c2.B) * (c1.B - c2.B));
        }
    }
}
