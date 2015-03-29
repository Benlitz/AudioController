using System;
using System.Runtime.InteropServices;

// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace AudioController.Native
{
    // NOTE: This enum is incomplete
    internal enum Message : uint
    {
        WM_KEYDOWN = 0x0100,
        WM_KEYUP = 0x0101,
        WM_CHAR = 0x0102,
        WM_DEADCHAR = 0x0103,
        WM_SYSKEYDOWN = 0x0104,
        WM_SYSKEYUP = 0x0105,
        WM_HOTKEY = 0x0312,
    }

    // NOTE: This enum is incomplete
    internal enum VK
    {
        KEY_I = 0x49,
        KEY_O = 0x4F,
        LWIN = 0x5B,
        RWIN = 0x5C,
    }

    [Flags]
    internal enum Modifier : uint
    {
        MOD_ALT = 0x1,
        MOD_CONTROL = 0x2,
        MOD_SHIFT = 0x4,
        MOD_WIN = 0x8,        
    }

    internal class Import
    {
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool RegisterHotKey(IntPtr hWnd, int id, Modifier fsModifiers, VK vk);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    }
}
