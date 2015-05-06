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
        private static readonly HotKey hotKey = new HotKey();
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

            Settings.Load();

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
            hotKey.Initialize();
            hotKey.HotKeyTriggered += SwitchAudioOutput;
            try
            {
                app = new App {ShutdownMode = ShutdownMode.OnExplicitShutdown};
                dispatcher = Dispatcher.CurrentDispatcher;
                app.Run();
                notifyIcon.Dispose();
            }
            finally
            {
                hotKey.Dispose();
            }
        }

        private static void ShowSettings()
        {
            if (settingsWindow == null)
            {
                settingsWindow = new SettingsWindow(hotKey);
                settingsWindow.Closed += (s, e) => settingsWindow = null;
                settingsWindow.Show();
            }
            settingsWindow.Activate();
        }

        private static void SwitchAudioOutput(object sender, EventArgs e)
        {
            var ignoreList = Settings.IgnoreList;
            var devices = Controller.GetDevices().Where(x => !ignoreList.Contains(x.Id)).ToList();
            if (devices.Count == 0)
                return;

            var currentDevice = Controller.GetDefaultDevice();
            var currentIndex = devices.IndexOf(currentDevice);

            var nextIndex = (currentIndex + 1) % devices.Count;
            if (nextIndex != currentIndex)
            {
                Controller.SetAsDefault(devices[nextIndex]);
                if (Settings.PlaySound)
                {
                    System.Media.SystemSounds.Beep.Play();
                }

                if (!Settings.ShowInFullscreen)
                {
                    var handle = Import.GetForegroundWindow();
                if (handle != IntPtr.Zero)
                {
                    RECT rect;
                    Import.GetWindowRect(handle, out rect);
                    var screen = MainWindow.GetTargetScreen();
                    if (screen != null)
                    {
                        if (screen.Bounds.Left == rect.Left && screen.Bounds.Right == rect.Right
                            && screen.Bounds.Top == rect.Top && screen.Bounds.Bottom == rect.Bottom)
                            return;
                    }
                }
                }
                dispatcher.BeginInvoke(new Action(() =>
                {
                    if (mainWindow != null)
                        mainWindow.Close();

                    string alias;
                    if (!Settings.Aliases.TryGetValue(devices[nextIndex].Id, out alias))
                        alias = devices[nextIndex].Name;

                    mainWindow = new MainWindow
                    {
                        DeviceName = alias
                    };
                    mainWindow.Show();
                }));
            }
        }
    }
}
