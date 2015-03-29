using System;
using System.Windows;
using System.Windows.Threading;

namespace AudioController
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(10)};
            timer.Tick += (sender, args) => Hide();
            timer.Start();
        }

        public string DeviceName { get; set; }
    }
}
