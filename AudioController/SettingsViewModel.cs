using System;
using WpfEx.ViewModels;

namespace AudioController
{
    public class SettingsViewModel : ViewModel
    {
        private readonly HotKey hotKey;
        private bool modifierCtrl;
        private bool modifierShift;
        private bool modifierAlt;
        private bool modifierWin;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsViewModel"/> class.
        /// </summary>
        /// <param name="hotKey">The hot key to set.</param>
        public SettingsViewModel(HotKey hotKey)
        {
            if (hotKey == null) throw new ArgumentNullException("hotKey");
            this.hotKey = hotKey;
            modifierCtrl = (Settings.Modifiers & Modifier.MOD_CONTROL) == Modifier.MOD_CONTROL;
            modifierShift = (Settings.Modifiers & Modifier.MOD_SHIFT) == Modifier.MOD_SHIFT;
            modifierAlt = (Settings.Modifiers & Modifier.MOD_ALT) == Modifier.MOD_ALT;
            modifierWin = (Settings.Modifiers & Modifier.MOD_WIN) == Modifier.MOD_WIN;
        }

        /// <summary>
        /// Gets or sets whether the Control key must be pressed to activate the hot key.
        /// </summary>
        public bool ModifierCtrl { get { return modifierCtrl; } set { SetValue(ref modifierCtrl, value); } }

        /// <summary>
        /// Gets or sets whether the Shift key must be pressed to activate the hot key.
        /// </summary>
        public bool ModifierShift { get { return modifierShift; } set { SetValue(ref modifierShift, value); } }

        /// <summary>
        /// Gets or sets whether the Alt key must be pressed to activate the hot key.
        /// </summary>
        public bool ModifierAlt { get { return modifierAlt; } set { SetValue(ref modifierAlt, value); } }

        /// <summary>
        /// Gets or sets whether the Windows key must be pressed to activate the hot key.
        /// </summary>
        public bool ModifierWin { get { return modifierWin; } set { SetValue(ref modifierWin, value); } }

        /// <summary>
        /// Applies the changes in the settings
        /// </summary>
        public void ApplySettings()
        {
            Settings.Modifiers = SetFlag(Settings.Modifiers, Modifier.MOD_CONTROL, ModifierCtrl);
            Settings.Modifiers = SetFlag(Settings.Modifiers, Modifier.MOD_SHIFT, ModifierShift);
            Settings.Modifiers = SetFlag(Settings.Modifiers, Modifier.MOD_ALT, ModifierAlt);
            Settings.Modifiers = SetFlag(Settings.Modifiers, Modifier.MOD_WIN, ModifierWin);
            hotKey.Update();
        }


        private static Modifier SetFlag(Modifier currentValue, Modifier flag, bool activate)
        {
            return activate ? currentValue | flag : currentValue & ~flag;
        }
    }
}
