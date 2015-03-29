using System;
using System.Collections.Generic;
using System.Linq;
using NAudio.CoreAudioApi;

namespace AudioController.Core
{
    public static class Controller
    {
        public static IEnumerable<Device> GetDevices()
        {
            var enumerator = new MMDeviceEnumerator();
            MMDeviceCollection endpoints = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.All);
            return endpoints.Where(x => x.State == DeviceState.Active).Select(device => new Device(device.FriendlyName, device.ID)).ToList();
        }

        public static Device GetDefaultDevice()
        {
            var enumerator = new MMDeviceEnumerator();
            var mmDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);
            return new Device(mmDevice.FriendlyName, mmDevice.ID);
        }

        public static bool SetAsDefault(Device device)
        {
            if (Environment.OSVersion.Version.Major < 6)
                throw new NotSupportedException("This method requires Windows Vista or above.");

            return Environment.OSVersion.Version.Minor == 0 ? SetAsDefaultVista(device) : SetAsDefaultSeven(device);
        }

        private static bool SetAsDefaultVista(Device device)
        {
            var client = new CPolicyConfigVistaClient();
            // ReSharper disable once SuspiciousTypeConversion.Global
            var policyConfig = client as IPolicyConfigVista;
            if (policyConfig != null)
            {
                policyConfig.SetDefaultEndpoint(device.Id, Role.Console);
                return true;
            }
            return false;            
        }

        private static bool SetAsDefaultSeven(Device device)
        {
            var client = new CPolicyConfigClient();
            // ReSharper disable once SuspiciousTypeConversion.Global
            var policyConfig = client as IPolicyConfig;
            if (policyConfig != null)
            {
                policyConfig.SetDefaultEndpoint(device.Id, Role.Console);
                return true;
            }
            return false;
        }
    }
}