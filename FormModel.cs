using SmartPixel.Extensions;
using SmartPixel.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using Color = System.Drawing.Color;

namespace SmartPixel
{
    public class FormModel : INotifyPropertyChanged
    {
        private string _path;
        private readonly FormService _formService;
        private int _colors = 20;
        private List<Color> _colorPalette = new List<Color>();
        private List<SolidColorBrush> _visualColorPalette = new List<SolidColorBrush>();

        public Relays StartSmartPixelCommand { get; private set; }
        public Relays ConvertCommand { get; private set; }
        public Relays GeneratePaletteCommand { get; private set; }
        public Relays GenerateGrayPaletteCommand { get; private set; }

        public FormModel()
        {
            _formService = new FormService();
            InitializeRelayCommands();
        }

        private void InitializeRelayCommands()
        {
            StartSmartPixelCommand = new Relays(StartSp, CanFireCommand);
            ConvertCommand = new Relays(ConvertSp, CanFireCommand);
            GeneratePaletteCommand = new Relays(GeneratePalette, CanFireCommand);
            GenerateGrayPaletteCommand = new Relays(GenerateGrayPalette, CanFireCommand);
        }

        public string Path
        {
            get => _path;
            set
            {
                if (_path == value) return;
                _path = value;
                RaisePropertyChanged("Path");
            }
        }

        public List<Color> ColorPalette
        {
            get => _colorPalette;
            set
            {
                if (_colorPalette == value) return;
                _colorPalette = value;
                RaisePropertyChanged("ColorPalette");
            }
        }

        public string Colors
        {
            get => $"{_colors}";
            set
            {
                if (_colors.ToString() == value) return;
                _colors = int.Parse(value);
                RaisePropertyChanged("Colors");
            }
        }

        public List<SolidColorBrush> VisualColorPalette
        {
            get => _visualColorPalette;
            set
            {
                if (_visualColorPalette == value) return;
                _visualColorPalette = value;
                RaisePropertyChanged("VisualColorPalette");
            }
        }

        private void StartSp(object input)
        {
            var path = _formService.StartSmartPixel();
            Path = path;
            GC.Collect();
        }

        private void ConvertSp(object input)
        {
            var path = _formService.SmartPixelConvert(Path, _colorPalette);
            Path = path;
        }

        private void GeneratePalette(object input)
        {
            var palette = _formService.GenerateColorPalette(_colors).ToList();
            ColorPalette = palette;
            GenerateVisualColorPalette();
        }

        private void GenerateGrayPalette(object input)
        {
            var palette = _formService.GenerateGrayColorPalette().ToList();
            ColorPalette = palette;
            GenerateVisualColorPalette();
        }

        private void GenerateVisualColorPalette()
        {
            var newVisualColorPalette = ColorPalette.Select(color => color.ConvertToMedia()).Select(mediaColor => new SolidColorBrush(mediaColor)).ToList();
            VisualColorPalette = newVisualColorPalette;
        }

        private bool CanFireCommand(object input)
        {
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string property = null)
        {
            if (property is null) return;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
