using System;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Threading;
using Color = System.Windows.Media.Color;
using Point = System.Drawing.Point;

namespace AudioController
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private Duration fadeOut;

        public MainWindow()
        {
            InitializeComponent();
            MainBorder.Background = GetBrush(Settings.BackgroundColor, Colors.LightSteelBlue);
            MainBorder.Opacity = Settings.Opacity;
            MainText.Foreground = GetBrush(Settings.TextColor, Colors.Black);
            Duration = TimeSpan.FromSeconds(Settings.Duration);
            FadeOut = TimeSpan.FromSeconds(Settings.FadeOut);
            var screen = GetTargetScreen();
            if (screen != null)
            {
                var centerX = screen.Bounds.Left + screen.Bounds.Width * 0.5;
                var centerY = screen.Bounds.Top + screen.Bounds.Height * 0.5;
                Width = screen.Bounds.Width * Settings.Size;
                Height = Width / 6.0;
                MainBorder.CornerRadius = new CornerRadius(Height / 3.0);
                // Hide everything if the height is too small
                if (Height < 15)
                    MainBorder.Visibility = Visibility.Hidden;

                Left = centerX - Width * 0.5;
                Top = centerY - Height * 0.5;
            }

            var timer = new DispatcherTimer { Interval = Duration + FadeOut.TimeSpan };
            timer.Tick += (sender, args) => Hide();
            timer.Start();
        }

        public string DeviceName { get; set; }

        public TimeSpan Duration { get; private set; }

        public Duration FadeOut { get { return fadeOut; } private set { fadeOut = value; } }

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

        public static Screen GetTargetScreen()
        {
            var screenId = Settings.DisplayOn;
            if (screenId == 0)
            {
                Point mousePos;
                Import.GetCursorPos(out mousePos);
                return Screen.FromPoint(mousePos);
            }
            foreach (var screen in Screen.AllScreens)
            {
                int index;
                if (int.TryParse(screen.DeviceName.Substring(@"\\.\DISPLAY".Length), out index) && index == screenId)
                {
                    return screen;
                }
            }
            return null;
        }
    }
}
