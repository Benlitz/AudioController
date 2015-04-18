using System.Windows;
using System.Windows.Forms;
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

        public SettingsViewModel ViewModel { get { return (SettingsViewModel)DataContext; } }

        private void CommandBindingClose(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter != null && (bool)e.Parameter)
            {
                ViewModel.ApplySettings();
            }
            Close();
        }
    }
}
