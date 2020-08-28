using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SmartPixel.Services
{
    public class ColorService
    {
        public Color GetNextColor(List<Color> palette)
        {
            var random = new Random();
            var colorBytes = new byte[3];
            Color finalColor;
            do
            {
                colorBytes[0] = (byte)random.Next(256);
                colorBytes[1] = (byte)random.Next(256);
                colorBytes[2] = (byte)random.Next(156);
                finalColor = Color.FromArgb(colorBytes[0], colorBytes[1], colorBytes[2]);
            } while (finalColor.R == finalColor.G && finalColor.G == finalColor.B && palette.Any(x => x.Equals(finalColor)));

            return finalColor;
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
            if (targetColor.A == 0) return Color.Transparent;
            var colorDiffs = colors.Select(n => ColorDiff(n, targetColor)).Min(n => n);
            return colors[colors.FindIndex(n => ColorDiff(n, targetColor) == colorDiffs)];
        }

        private int ColorDiff(Color c1, Color c2)
        {
            return (int)Math.Sqrt(Math.Pow((c1.R - c2.R), 2) + Math.Pow((c1.G - c2.G), 2) + Math.Pow((c1.B - c2.B), 2));
        }
    }
}
