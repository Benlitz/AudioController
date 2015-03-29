using System;
using System.Windows.Interop;

namespace AudioController
{
    public class HotKey : IDisposable
    {
        public event EventHandler<EventArgs> HotKeyTriggered;

        public void Initialize()
        {
            Import.RegisterHotKey(IntPtr.Zero, GetHashCode(), Modifier.MOD_WIN, VK.KEY_O);
            ComponentDispatcher.ThreadPreprocessMessage += ComponentDispatcherThreadPreprocessMessage;
        }

        public void Dispose()
        {
            Import.UnregisterHotKey(IntPtr.Zero, GetHashCode());
        }

        private void ComponentDispatcherThreadPreprocessMessage(ref MSG msg, ref bool handled)
        {
            // ReSharper disable once InconsistentNaming
            const int WM_HOTKEY = 0x0312;

            if (msg.message == WM_HOTKEY)
            {
                var handler = HotKeyTriggered;
                if (handler != null)
                    handler(this, EventArgs.Empty);
    
                handled = true;
            }
        }
    }
}
