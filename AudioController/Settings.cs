using System;
using System.Collections.Generic;
using System.Linq;

namespace AudioController
{
    /// <summary>
    /// A static class that allows to read/write settings from the settings file.
    /// </summary>
    public static class Settings
    {
        private static readonly HashSet<string> ignoreList = new HashSet<string>();
        private static readonly Dictionary<string, string> aliases = new Dictionary<string, string>();
        public const string StringArraySeparator = "|$^%|";
        public const string AliasAssignmentToken = "//=:=//";

        /// <summary>
        /// Gets or sets the modifiers of the hot key.
        /// </summary>
        public static Modifier Modifiers { get; set; }

        /// <summary>
        /// Gets or sets the key of the hot key.
        /// </summary>
        public static VirtualKey Key { get; set; }

        /// <summary>
        /// Gets the list of ids of the devices that are ignored.
        /// </summary>
        public static ISet<string> IgnoreList { get { return ignoreList; } }

        /// <summary>
        /// Gets the dictionary that maps a device id to its alias.
        /// </summary>
        public static IDictionary<string, string> Aliases { get { return aliases; } }

        /// <summary>
        /// Gets or sets the background color of the notification window.
        /// </summary>
        public static string BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the text color of the notification window.
        /// </summary>
        public static string TextColor { get; set; }

        /// <summary>
        /// Gets or sets the opacity of the notification window.
        /// </summary>
        public static double Opacity { get; set; }

        /// <summary>
        /// Gets or sets the size of the notifiction window, as a percentage of the resolution of the display it appears on.
        /// </summary>
        public static double Size { get; set; }

        /// <summary>
        /// Gets or sets on which display the notification window should appear.
        /// </summary>
        public static int DisplayOn { get; set; }

        /// <summary>
        /// Gets or sets whether to display the notification window when there is a fullscreen application
        /// </summary>
        public static bool ShowInFullscreen { get; set; }

        /// <summary>
        /// Gets or sets whether to play a sound when the default device is modified.
        /// </summary>
        public static bool PlaySound { get; set; }

        /// <summary>
        /// Gets or sets the duration of the notification at maximal opacity.
        /// </summary>
        public static int Duration { get; set; }

        /// <summary>
        /// Gets or sets the fade out time of the notification.
        /// </summary>
        public static int FadeOut { get; set; }

        public static void Load()
        {
            try
            {
                Modifier modifiers;
                Enum.TryParse(Properties.Settings.Default.Modifiers, out modifiers);
                Modifiers = modifiers;
                VirtualKey key;
                Enum.TryParse(Properties.Settings.Default.Key, out key);
                Key = key;
                ignoreList.Clear();
                foreach (var ignoredDevice in Split(Properties.Settings.Default.IgnoreList, StringArraySeparator))
                {
                    ignoreList.Add(ignoredDevice);
                }
                aliases.Clear();
                foreach (var alias in Split(Properties.Settings.Default.AliasList, StringArraySeparator))
                {
                    var values = Split(alias, AliasAssignmentToken);
                    Aliases.Add(values[0], values[1]);
                }
                BackgroundColor = Properties.Settings.Default.BackgroundColor;
                TextColor = Properties.Settings.Default.TextColor;
                Opacity = Properties.Settings.Default.Opacity;
                Size = Properties.Settings.Default.Size;
                DisplayOn = Properties.Settings.Default.DisplayOn;
                ShowInFullscreen = Properties.Settings.Default.ShowInFullscreen;
                PlaySound = Properties.Settings.Default.PlaySound;
                Duration = Properties.Settings.Default.Duration;
                FadeOut = Properties.Settings.Default.FadeOut;
            }
            catch (Exception)
            {
                ignoreList.Clear();
                aliases.Clear();
            }
        }

        public static void Save()
        {
            var aliasStrings = Aliases.Select(x => string.Format("{0}{1}{2}", x.Key, AliasAssignmentToken, x.Value));
            Properties.Settings.Default.Modifiers = Modifiers.ToString();
            Properties.Settings.Default.Key = Key.ToString();
            Properties.Settings.Default.IgnoreList = string.Join(StringArraySeparator, ignoreList);
            Properties.Settings.Default.AliasList = string.Join(StringArraySeparator, aliasStrings);
            Properties.Settings.Default.BackgroundColor = BackgroundColor;
            Properties.Settings.Default.TextColor = TextColor;
            Properties.Settings.Default.Opacity = Opacity;
            Properties.Settings.Default.Size = Size;
            Properties.Settings.Default.DisplayOn = DisplayOn;
            Properties.Settings.Default.ShowInFullscreen = ShowInFullscreen;
            Properties.Settings.Default.PlaySound = PlaySound;
            Properties.Settings.Default.Duration = Duration;
            Properties.Settings.Default.FadeOut = FadeOut;
            Properties.Settings.Default.Save();
        }

        private static string[] Split(string str, string token)
        {
            return str.Split(new[] {token}, StringSplitOptions.RemoveEmptyEntries);

        }
    }
}