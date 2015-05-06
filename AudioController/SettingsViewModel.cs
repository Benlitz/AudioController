using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using AudioController.Core;
using WpfEx.ViewModels;

namespace AudioController
{
    public class DisplayViewModel : ViewModel
    {
        public DisplayViewModel(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; private set; }

        public string Name { get; private set; }
    }

    public class SettingsViewModel : ViewModel
    {
        private readonly HotKey hotKey;
        private readonly List<VirtualKey> availableKeys;
        private readonly List<DeviceViewModel> devices;
        private readonly List<ColorViewModel> availableColors;
        private readonly List<DisplayViewModel> availableTargetDisplay;
        private bool modifierCtrl;
        private bool modifierShift;
        private bool modifierAlt;
        private bool modifierWin;
        private VirtualKey key;
        private DeviceViewModel selectedDevice;
        private ColorViewModel backgroundColor;
        private ColorViewModel textColor;
        private double opacity;
        private bool playSound;
        private DisplayViewModel targetDisplay;
        private double size;
        private bool showInFullscreen;

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
            opacity = Settings.Opacity;
            size = Settings.Size;
            availableTargetDisplay = new List<DisplayViewModel> { new DisplayViewModel(0, "Display with mouse cursor") };
            foreach (var screen in Screen.AllScreens)
            {
                string monitorName = null;
                var displayDevice = new DISPLAY_DEVICE();
                displayDevice.cb = Marshal.SizeOf(displayDevice);
                if (Import.EnumDisplayDevices(screen.DeviceName, 0, ref displayDevice, 0x00000001))
                {
                    monitorName = displayDevice.DeviceString;
                }

                int index;
                if (int.TryParse(screen.DeviceName.Substring(@"\\.\DISPLAY".Length), out index))
                {
                    var name = !string.IsNullOrEmpty(monitorName) ? string.Format("Display {0} ({1})", index, monitorName) : string.Format("Display {0}", index);
                    availableTargetDisplay.Add(new DisplayViewModel(index, name));
                }
            }
            TargetDisplay = availableTargetDisplay.FirstOrDefault(x => x.Id == Settings.DisplayOn) ?? availableTargetDisplay.FirstOrDefault();
            showInFullscreen = Settings.ShowInFullscreen;
            playSound = Settings.PlaySound;
            ApplySettingsCommand = new AnonymousCommand(ApplySettings);
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
        /// Gets or sets the opacity of the notification window.
        /// </summary>
        public double Opacity { get { return opacity; } set { SetValue(ref opacity, value); } }

        /// <summary>
        /// Gets or sets the size of the notification window, in percentage of the target screen.
        /// </summary>
        public double Size { get { return size; } set { SetValue(ref size, value); } }

        /// <summary>
        /// Gets the collection of display names on which the notification window can be displayed.
        /// </summary>
        public IEnumerable<DisplayViewModel> AvailableTargetDisplay { get { return availableTargetDisplay; } }

        /// <summary>
        /// Gets or sets the name of the display on which the notification window can be displayed.
        /// </summary>
        public DisplayViewModel TargetDisplay { get { return targetDisplay; } set { SetValue(ref targetDisplay, value); } }

        /// <summary>
        /// Gets or sets whether to display the notification window when there is an application running in fullscreen on the target display.
        /// </summary>
        public bool ShowInFullscreen { get { return showInFullscreen; } set { SetValue(ref showInFullscreen, value); } }

        /// <summary>
        /// Gets or sets whether to play a sound when the default device is changed.
        /// </summary>
        public bool PlaySound { get { return playSound; } set { SetValue(ref playSound, value); } }

        /// <summary>
        /// Gets the command that will apply the settings of the view model to the settings of the application.
        /// </summary>
        public ICommand ApplySettingsCommand { get; private set; }

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
            Settings.Opacity = Opacity;
            Settings.Size = Size;
            Settings.DisplayOn = TargetDisplay != null ? TargetDisplay.Id : 0;
            Settings.ShowInFullscreen = ShowInFullscreen;
            Settings.PlaySound = PlaySound;
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
