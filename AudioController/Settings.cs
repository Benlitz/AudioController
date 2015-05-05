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
            }
        }

        /// <summary>
        /// Gets the list of ids of the devices that are ignored.
        /// </summary>
        public static ISet<string> IgnoreList { get { return ignoreList; } }

        /// <summary>
        /// Gets the dictionary that maps a device id to its alias.
        /// </summary>
        public static IDictionary<string, string> Aliases { get { return aliases; } }

        public static void Load()
        {
            try
            {
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
            Properties.Settings.Default.IgnoreList = string.Join(StringArraySeparator, ignoreList);
            Properties.Settings.Default.AliasList = string.Join(StringArraySeparator, aliasStrings);
            Properties.Settings.Default.Save();
        }

        private static string[] Split(string str, string token)
        {
            return str.Split(new[] {token}, StringSplitOptions.RemoveEmptyEntries);

        }
    }
}