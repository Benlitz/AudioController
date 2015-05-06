using System;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace AudioController
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            MainBorder.Background = GetBrush(Settings.BackgroundColor, Colors.LightSteelBlue);
            MainBorder.Opacity = Settings.Opacity;
            MainText.Foreground = GetBrush(Settings.TextColor, Colors.Black);
            var timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(10)};
            timer.Tick += (sender, args) => Hide();
            timer.Start();
        }

        public string DeviceName { get; set; }

        private SolidColorBrush GetBrush(string colorName, Color fallback)
        {
            try
            {
                var colorProperty = typeof(Colors).GetProperty(colorName, BindingFlags.Public | BindingFlags.Static);
                var color = (Color)colorProperty.GetValue(null);
                return new SolidColorBrush(color);
            }
            catch (Exception)
            {
                return new SolidColorBrush(fallback);
            }
        }
    }
}
