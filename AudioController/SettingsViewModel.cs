using System;
using WpfEx.ViewModels;

namespace AudioController
{
    public class SettingsViewModel : ViewModel
    {
        private readonly HotKey hotKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsViewModel"/> class.
        /// </summary>
        /// <param name="hotKey">The hot key to set.</param>
        public SettingsViewModel(HotKey hotKey)
        {
            if (hotKey == null) throw new ArgumentNullException("hotKey");
            this.hotKey = hotKey;
        }

        /// <summary>
        /// Gets or sets whether the Control key must be pressed to activate the hot key.
        /// </summary>
        public bool ModifierCtrl
        {
            get { return (hotKey.Modifiers & Modifier.MOD_CONTROL) == Modifier.MOD_CONTROL; }
            set { SetValue(ref hotKey.Modifiers, SetFlag(hotKey.Modifiers, Modifier.MOD_CONTROL, value), hotKey.Update); }
        }

        /// <summary>
        /// Gets or sets whether the Shift key must be pressed to activate the hot key.
        /// </summary>
        public bool ModifierShift
        {
            get { return (hotKey.Modifiers & Modifier.MOD_SHIFT) == Modifier.MOD_SHIFT; }
            set { SetValue(ref hotKey.Modifiers, SetFlag(hotKey.Modifiers, Modifier.MOD_SHIFT, value), hotKey.Update); }
        }

        /// <summary>
        /// Gets or sets whether the Alt key must be pressed to activate the hot key.
        /// </summary>
        public bool ModifierAlt
        {
            get { return (hotKey.Modifiers & Modifier.MOD_ALT) == Modifier.MOD_ALT; }
            set { SetValue(ref hotKey.Modifiers, SetFlag(hotKey.Modifiers, Modifier.MOD_ALT, value), hotKey.Update); }
        }

        /// <summary>
        /// Gets or sets whether the Windows key must be pressed to activate the hot key.
        /// </summary>
        public bool ModifierWin
        {
            get { return (hotKey.Modifiers & Modifier.MOD_WIN) == Modifier.MOD_WIN; }
            set { SetValue(ref hotKey.Modifiers, SetFlag(hotKey.Modifiers, Modifier.MOD_WIN, value), hotKey.Update); }
        }

        private static Modifier SetFlag(Modifier currentValue, Modifier flag, bool activate)
        {
            return activate ? currentValue | flag : currentValue & ~flag;
        }
    }
}
