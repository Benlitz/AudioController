using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using AudioController.Core;

using WinForms = System.Windows.Forms;

namespace AudioController
{
    public static class Program
    {
        private static readonly HotKey HotKey = new HotKey();
        private static App app;
        private static MainWindow mainWindow;
        private static Dispatcher dispatcher;
        private static SettingsWindow settingsWindow;

        [STAThread]
        [DebuggerNonUserCode]
        public static void Main()
        {
            WinForms.Application.EnableVisualStyles();
            WinForms.Application.SetCompatibleTextRenderingDefault(false);

            var notifyIcon = new WinForms.NotifyIcon
            {
                Visible = true,
                Icon = Properties.Resources.Icon,
                ContextMenu = new WinForms.ContextMenu(new[]
                {
                    new WinForms.MenuItem("Settings", (s, e) => ShowSettings()),
                    new WinForms.MenuItem("-"),
                    new WinForms.MenuItem("Exit", (s, e) => app.Shutdown())
                })
            };
            notifyIcon.DoubleClick += (s, e) => ShowSettings();
            HotKey.Initialize();
            HotKey.HotKeyTriggered += SwitchAudioOutput;
            try
            {
                app = new App {ShutdownMode = ShutdownMode.OnExplicitShutdown};
                dispatcher = Dispatcher.CurrentDispatcher;
                app.Run();
                notifyIcon.Dispose();
            }
            finally
            {
                HotKey.Dispose();
            }
        }

        private static void ShowSettings()
        {
            if (settingsWindow == null)
            {
                settingsWindow = new SettingsWindow(HotKey);
                settingsWindow.Closed += (s, e) => settingsWindow = null;
                settingsWindow.Show();
            }
            settingsWindow.Activate();
        }

        private static void SwitchAudioOutput(object sender, EventArgs e)
        {
            var ignoreList = Settings.IgnoreList;
            var devices = Controller.GetDevices().Where(x => !ignoreList.Contains(x.Name)).ToList();
            if (devices.Count == 0)
                return;

            var currentDevice = Controller.GetDefaultDevice();
            var currentIndex = devices.IndexOf(currentDevice);

            var nextIndex = (currentIndex + 1) % devices.Count;
            if (nextIndex != currentIndex)
            {
                Controller.SetAsDefault(devices[nextIndex]);
                dispatcher.BeginInvoke(new Action(() =>
                {
                    if (mainWindow != null)
                        mainWindow.Close();

                    mainWindow = new MainWindow
                    {
                        DeviceName = devices[nextIndex].Name
                    };
                    mainWindow.Show();
                }));
            }
        }
    }
}
