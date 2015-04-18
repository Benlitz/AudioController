using System;

namespace AudioController
{
    /// <summary>
    /// A static class that allows to read/write settings from the settings file.
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// Gets or sets the modifiers of the hot key.
        /// </summary>
        public static Modifier Modifiers
        {
            get { Modifier result; Enum.TryParse(Properties.Settings.Default.Modifiers, out result); return result; }
            set { Properties.Settings.Default.Modifiers = value.ToString(); Save(); }
        }

        private static void Save()
        {
            Properties.Settings.Default.Save();
        }
    }
}