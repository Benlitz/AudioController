using System.Windows;
using System.Windows.Input;

namespace AudioController
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow
    {
        public SettingsWindow(HotKey hotKey)
        {
            DataContext = new SettingsViewModel(hotKey);
            InitializeComponent();
        }

        private void CommandBindingClose(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }
    }
}
