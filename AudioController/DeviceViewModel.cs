using System;
using System.Linq;
using System.Windows.Input;
using AudioController.Core;
using WpfEx.ViewModels;
using XfNet;

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
            Alias = Name;
            ResetCommand = new AnonymousCommand(() => Alias = Name);
        }

        public string Name { get { return device.Name; } }

        public string Alias { get { return alias; } set { SetValue(ref alias, value); } }

        public bool Ignore { get { return ignore; } set { SetValue(ref ignore, value); } }

        public ICommand ResetCommand { get; private set; }

        public void ApplySettings()
        {
            var ignoreList = Settings.IgnoreList;
            if (Ignore && !ignoreList.Contains(device.Id))
            {
                ignoreList = ignoreList.Concat(device.Id.Yield()).ToArray();
                Settings.IgnoreList = ignoreList;
            }
            if (!Ignore && ignoreList.Contains(device.Id))
            {
                ignoreList = ignoreList.Where(x => x != device.Id).ToArray();
                Settings.IgnoreList = ignoreList;
            }
        }
    }
}