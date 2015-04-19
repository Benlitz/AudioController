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
        public const string StringArraySeparator = "|$^%|";
        public const string AliasAssignmentToken = "//=:=//";

        /// <summary>
        /// Gets or sets the modifiers of the hot key.
        /// </summary>
        public static Modifier Modifiers
        {
            get
            {
                Modifier result;
                Enum.TryParse(Properties.Settings.Default.Modifiers, out result);
                return result;
            }
            set
            {
                Properties.Settings.Default.Modifiers = value.ToString();
                Save();
            }
        }

        /// <summary>
        /// Gets or sets the key of the hot key.
        /// </summary>
        public static VirtualKey Key
        {
            get
            {
                VirtualKey result;
                Enum.TryParse(Properties.Settings.Default.Key, out result);
                return result;
            }
            set
            {
                Properties.Settings.Default.Key = value.ToString();
                Save();
            }
        }

        /// <summary>
        /// Gets or sets the ignore list.
        /// </summary>
        public static string[] IgnoreList
        {
            get { return Properties.Settings.Default.IgnoreList.Split(new[] {StringArraySeparator}, StringSplitOptions.RemoveEmptyEntries); }
            set
            {
                Properties.Settings.Default.IgnoreList = string.Join(StringArraySeparator, value);
                Save();
            }
        }

        private static void Save()
        {
            Properties.Settings.Default.Save();
        }
    }
}