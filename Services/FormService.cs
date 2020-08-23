using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Color = System.Drawing.Color;

namespace SmartPixel.Services
{
    public class FormService
    {
        private readonly ColorService _cgs = new ColorService();

        public string StartSmartPixel()
        {
            var fd = new OpenFileDialog();
            fd.ShowDialog();
            return !fd.CheckPathExists ? string.Empty : fd.FileName;
        }

        public string SmartPixelConvert(string path, List<Color> palette)
        {
            if (string.IsNullOrEmpty(path)) return string.Empty;
            var uploadedImg = Image.FromFile(path);
            var uploadedBit = new DirectBitmap(uploadedImg);
            uploadedImg.Dispose();
            var newBit = new DirectBitmap(uploadedBit.Width, uploadedBit.Height);

            for (var x = 1; x <= uploadedBit.Width; ++x)
            {
                for (var y = 1; y <= uploadedBit.Height; ++y)
                {
                    var targetPixel = uploadedBit.Bitmap.GetPixel(x - 1, y - 1);
                    var targetColor = ReturnClosestColor(targetPixel, palette);
                    newBit.SetPixel(x - 1, y - 1, targetColor);
                }
            }
            uploadedBit.Dispose();

            var time = $"{DateTimeOffset.Now.Day}{DateTimeOffset.Now.Hour}{DateTimeOffset.Now.Minute}{DateTimeOffset.Now.Second}";
            if (!Directory.Exists($@"{Environment.CurrentDirectory}\GeneratedImages"))
            {
                Directory.CreateDirectory($@"{Environment.CurrentDirectory}\GeneratedImages");
            }
            newBit.Bitmap.Save($@"{Environment.CurrentDirectory}\GeneratedImages\GENERATED{time}.png", ImageFormat.Png);
            newBit.Dispose();
            return @$"{Environment.CurrentDirectory}\GeneratedImages\GENERATED{time}.png";
        }

        // Generates a random color palette. Could be bad, could be good.
        public IEnumerable<Color> GenerateColorPalette()
        {
            var palette = new List<Color>();
            for (var i = 0; i < 16; i++)
            {
                var color = _cgs.GetNextColor();
                palette.Add(color);
            }
            return palette;
        }

        // Generates a random color palette. Could be bad, could be good.
        public IEnumerable<Color> GenerateGrayColorPalette()
        {
            var palette = new List<Color>();
            for (var i = 0; i < 16; i++)
            {
                var color = _cgs.GetNextGrayColor();
                palette.Add(color);
            }
            return palette;
        }

        private Color ReturnClosestColor(Color targetColor, List<Color> newColors)
        {
            return _cgs.SelectClosestColor(newColors, targetColor);
        }
    }
}