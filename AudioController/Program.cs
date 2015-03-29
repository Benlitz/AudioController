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
        private static readonly HotKey Hook = new HotKey();
        private static App app;
        private static MainWindow mainWindow;
        private static Dispatcher dispatcher;

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
                    new WinForms.MenuItem("Exit", (s, e) => app.Shutdown()),
                })
            };

            Hook.Initialize();
            Hook.HotKeyTriggered += SwitchAudioOutput;
            try
            {
                app = new App {ShutdownMode = ShutdownMode.OnExplicitShutdown};
                dispatcher = Dispatcher.CurrentDispatcher;
                app.Run();
                notifyIcon.Dispose();
            }
            finally
            {
                Hook.Dispose();
            }
        }

        private static void SwitchAudioOutput(object sender, EventArgs e)
        {
            var devices = Controller.GetDevices().ToList();
            var currentDevice = Controller.GetDefaultDevice();
            var currentIndex = devices.IndexOf(currentDevice);
            if (currentIndex < 0)
            {
                throw new InvalidOperationException("Unable to retrieve the current default audio output.");
            }
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
