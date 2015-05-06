using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Media;
using AudioController.Core;
using WpfEx.ViewModels;

namespace AudioController
{
    public class SettingsViewModel : ViewModel
    {
        private readonly HotKey hotKey;
        private readonly List<VirtualKey> availableKeys;
        private readonly List<DeviceViewModel> devices;
        private readonly List<ColorViewModel> availableColors;
        private bool modifierCtrl;
        private bool modifierShift;
        private bool modifierAlt;
        private bool modifierWin;
        private VirtualKey key;
        private DeviceViewModel selectedDevice;
        private ColorViewModel backgroundColor;
        private ColorViewModel textColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsViewModel"/> class.
        /// </summary>
        /// <param name="hotKey">The hot key to set.</param>
        public SettingsViewModel(HotKey hotKey)
        {
            if (hotKey == null) throw new ArgumentNullException("hotKey");
            this.hotKey = hotKey;
            var modifiers = Settings.Modifiers;
            modifierCtrl = (modifiers & Modifier.MOD_CONTROL) == Modifier.MOD_CONTROL;
            modifierShift = (modifiers & Modifier.MOD_SHIFT) == Modifier.MOD_SHIFT;
            modifierAlt = (modifiers & Modifier.MOD_ALT) == Modifier.MOD_ALT;
            modifierWin = (modifiers & Modifier.MOD_WIN) == Modifier.MOD_WIN;
            availableKeys = new List<VirtualKey>(Enum.GetValues(typeof(VirtualKey)).Cast<VirtualKey>()
                .Where(x => x >= VirtualKey.KEY_0 && x <= VirtualKey.KEY_Z));
            var settingsKey = Settings.Key;
            key = settingsKey;
            devices = new List<DeviceViewModel>(Controller.GetDevices().Select(x => new DeviceViewModel(x)));
            var colors = typeof(Colors).GetProperties(BindingFlags.Static | BindingFlags.Public).Where(x => x.PropertyType == typeof(Color));
            availableColors = new List<ColorViewModel>(colors.Select(x => new ColorViewModel(x.Name, (Color)x.GetValue(null))));
            availableColors.Sort();
            backgroundColor = availableColors.FirstOrDefault(x => x.Name == Settings.BackgroundColor);
            textColor = availableColors.FirstOrDefault(x => x.Name == Settings.TextColor);
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
        /// Gets the collection of available keys for the hot key.
        /// </summary>
        public IEnumerable<VirtualKey> AvailableKeys { get { return availableKeys; } }

        /// <summary>
        /// Gets or sets the key to use in combinaison with modifiers to trigger the hot key.
        /// </summary>
        public VirtualKey Key { get { return key; } set { SetValue(ref key, value); } }

        /// <summary>
        /// Gets the collection of available devices.
        /// </summary>
        public IEnumerable<DeviceViewModel> Devices { get { return devices; } }

        /// <summary>
        /// Gets or sets the selected device.
        /// </summary>
        public DeviceViewModel SelectedDevice { get { return selectedDevice; } set { SetValue(ref selectedDevice, value); } }

        /// <summary>
        /// Gets the collection of available colors.
        /// </summary>
        public IEnumerable<ColorViewModel> AvailableColors { get { return availableColors; } }

        /// <summary>
        /// Gets or sets the background color of the notification window.
        /// </summary>
        public ColorViewModel BackgroundColor { get { return backgroundColor; } set { SetValue(ref backgroundColor, value); } }

        /// <summary>
        /// Gets or sets the text color of the notification window.
        /// </summary>
        public ColorViewModel TextColor { get { return textColor; } set { SetValue(ref textColor, value); } }

        /// <summary>
        /// Applies the changes in the settings.
        /// </summary>
        public void ApplySettings()
        {
            var modifiers = (Modifier)0;
            modifiers = SetFlag(modifiers, Modifier.MOD_CONTROL, ModifierCtrl);
            modifiers = SetFlag(modifiers, Modifier.MOD_SHIFT, ModifierShift);
            modifiers = SetFlag(modifiers, Modifier.MOD_ALT, ModifierAlt);
            modifiers = SetFlag(modifiers, Modifier.MOD_WIN, ModifierWin);
            Settings.Modifiers = modifiers;
            Settings.Key = Key;
            Settings.BackgroundColor = BackgroundColor != null ? BackgroundColor.Name : "";
            Settings.TextColor = TextColor != null ? TextColor.Name : "";
            foreach (var device in devices)
            {
                device.ApplySettings();
            }
            Settings.Save();

            hotKey.Update();
        }


        private static Modifier SetFlag(Modifier currentValue, Modifier flag, bool activate)
        {
            return activate ? currentValue | flag : currentValue & ~flag;
        }
    }
}
