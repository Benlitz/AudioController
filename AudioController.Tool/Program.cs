using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace AudioController.Tool
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
            Hook.Initialize();
            Hook.HotKeyTriggered += SwitchAudioOutput;
            try
            {
                app = new App {ShutdownMode = ShutdownMode.OnExplicitShutdown};
                dispatcher = Dispatcher.CurrentDispatcher;
                app.Run();
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
