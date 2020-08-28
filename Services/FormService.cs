using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
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
            var uploadedBit = new Bitmap(uploadedImg);
            uploadedImg.Dispose();
            unsafe
            {
                var uploadedBitData = uploadedBit.LockBits(new Rectangle(0, 0, uploadedBit.Width, uploadedBit.Height), ImageLockMode.ReadWrite, uploadedBit.PixelFormat);
                var bytesPerPixel = Image.GetPixelFormatSize(uploadedBit.PixelFormat) / 8;
                var heightPx = uploadedBitData.Height;
                var widthBytes = uploadedBitData.Width * bytesPerPixel;
                var ptrFirstPx = (byte*)uploadedBitData.Scan0;

                Parallel.For(0, heightPx, y =>
                {
                    var currentLine = ptrFirstPx + (y * uploadedBitData.Stride);
                    for (var x = 0; x < widthBytes; x += bytesPerPixel)
                    {
                        int originalA = currentLine[x + 3];
                        int originalR = currentLine[x];
                        int originalG = currentLine[x + 1];
                        int originalB = currentLine[x + 2];
                        var originalColor = Color.FromArgb(originalR, originalG, originalB);
                        var closestColor = ReturnClosestColor(originalColor, palette);
                        currentLine[x + 3] = (byte)originalA;
                        currentLine[x] = closestColor.R;
                        currentLine[x + 1] = closestColor.G;
                        currentLine[x + 2] = closestColor.B;
                    }
                });
                uploadedBit.UnlockBits(uploadedBitData);
            }


            var time = $"{DateTimeOffset.Now.Day}{DateTimeOffset.Now.Hour}{DateTimeOffset.Now.Minute}{DateTimeOffset.Now.Second}";
            if (!Directory.Exists($@"{Environment.CurrentDirectory}\GeneratedImages"))
            {
                Directory.CreateDirectory($@"{Environment.CurrentDirectory}\GeneratedImages");
            }
            uploadedBit.Save($@"{Environment.CurrentDirectory}\GeneratedImages\GENERATED{time}.png", ImageFormat.Png);
            uploadedBit.Dispose();
            return @$"{Environment.CurrentDirectory}\GeneratedImages\GENERATED{time}.png";
        }

        // Generates a random color palette. Could be bad, could be good.
        public IEnumerable<Color> GenerateColorPalette(int amount)
        {
            var palette = new List<Color>();
            for (var i = 0; i < amount; i++)
            {
                var color = _cgs.GetNextColor(palette);
                palette.Add(color);
            }
            return palette;
        }

        // Generates a random color palette. Could be bad, could be good.
        public IEnumerable<Color> GenerateGrayColorPalette()
        {
            var palette = new List<Color>();
            for (var i = 0; i < 20; i++)
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