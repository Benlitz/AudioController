using System;
using System.Windows.Input;
using AudioController.Core;
using WpfEx.ViewModels;

namespace AudioController
{
    public class DeviceViewModel : ViewModel
    {
        private readonly Device device;
        private string alias;
        private bool ignore;

        public DeviceViewModel(Device device)
        {
            if (device == null) throw new ArgumentNullException("device");
            this.device = device;
            ignore = Settings.IgnoreList.Contains(device.Id);
            if (!Settings.Aliases.TryGetValue(device.Id, out alias))
                alias = Name;
            ResetCommand = new AnonymousCommand(() => Alias = Name);
        }

        public string Name { get { return device.Name; } }

        public string Alias { get { return alias; } set { SetValue(ref alias, value); } }

        public bool Ignore { get { return ignore; } set { SetValue(ref ignore, value); } }

        public ICommand ResetCommand { get; private set; }

        public void ApplySettings()
        {
            if (Ignore)
                Settings.IgnoreList.Add(device.Id);
            else
                Settings.IgnoreList.Remove(device.Id);

            if (Alias != Name)
                Settings.Aliases[device.Id] = Alias;
            else
                Settings.Aliases.Remove(device.Id);
        }
    }
}