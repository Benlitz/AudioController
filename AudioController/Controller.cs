using System;
using System.Collections.Generic;
using System.Linq;
using AudioController.Native;
using NAudio.CoreAudioApi;

namespace AudioController
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
            var policyConfigClientGuid = new Guid("294935CE-F637-4E7C-A41B-AB255460B862");
            var policyConfigGuid = new Guid("568b9108-44bf-40b4-9006-86afe5b5a620");

            var policyConfig = Import.CoCreateInstance(policyConfigClientGuid, null, CLSCTX.CLSCTX_ALL, policyConfigGuid) as IPolicyConfigVista;
            if (policyConfig != null)
            {
                policyConfig.SetDefaultEndpoint(device.Id, ERole.eConsole);
                return true;
            }
            return false;            
        }

        private static bool SetAsDefaultSeven(Device device)
        {
            var policyConfigClientGuid = new Guid("870af99c-171d-4f9e-af0d-e63df40c2bc9");
            var policyConfigGuid = new Guid("f8679f50-850a-41cf-9c72-430f290290c8");

            var policyConfig = Import.CoCreateInstance(policyConfigClientGuid, null, CLSCTX.CLSCTX_ALL, policyConfigGuid) as IPolicyConfig;
            if (policyConfig != null)
            {
                policyConfig.SetDefaultEndpoint(device.Id, ERole.eConsole);
                return true;
            }
            return false;
        }
    }
}