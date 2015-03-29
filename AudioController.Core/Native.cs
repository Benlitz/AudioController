using System;
using System.Runtime.InteropServices;
using NAudio.CoreAudioApi;

namespace AudioController.Core
{
    [ComImport, Guid("870af99c-171d-4f9e-af0d-e63df40c2bc9")]
    internal class CPolicyConfigClient
    {      
    }

    [ComImport, Guid("294935CE-F637-4E7C-A41B-AB255460B862")]
    internal class CPolicyConfigVistaClient
    {
    }

    [Guid("f8679f50-850a-41cf-9c72-430f290290c8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IPolicyConfig
    {
        [PreserveSig]
        int GetMixFormat(string pszDeviceName, IntPtr ppFormat);
        [PreserveSig]
        int GetDeviceFormat(string pszDeviceName, bool bDefault, IntPtr ppFormat);
        [PreserveSig]
        int ResetDeviceFormat(string pszDeviceName);
        [PreserveSig]
        int SetDeviceFormat(string pszDeviceName, IntPtr pEndpointFormat, IntPtr mixFormat);
        [PreserveSig]
        int GetProcessingPeriod(string pszDeviceName, bool bDefault, IntPtr pmftDefaultPeriod, IntPtr pmftMinimumPeriod);
        [PreserveSig]
        int SetProcessingPeriod(string pszDeviceName, IntPtr pmftPeriod);
        [PreserveSig]
        int GetShareMode(string pszDeviceName, IntPtr pMode);
        [PreserveSig]
        int SetShareMode(string pszDeviceName, IntPtr mode);
        [PreserveSig]
        int GetPropertyValue(string pszDeviceName, bool bFxStore, IntPtr key, IntPtr pv);
        [PreserveSig]
        int SetPropertyValue(string pszDeviceName, bool bFxStore, IntPtr key, IntPtr pv);
        [PreserveSig]
        int SetDefaultEndpoint(string pszDeviceName, Role role);
        [PreserveSig]
        int SetEndpointVisibility(string pszDeviceName, bool bVisible);
    }

    [Guid("568b9108-44bf-40b4-9006-86afe5b5a620"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IPolicyConfigVista
    {
        [PreserveSig]
        int GetMixFormat(string pszDeviceName, IntPtr ppFormat);
        [PreserveSig]
        int GetDeviceFormat(string pszDeviceName, bool bDefault, IntPtr ppFormat);
        [PreserveSig]
        int ResetDeviceFormat(string pszDeviceName);
        [PreserveSig]
        int SetDeviceFormat(string pszDeviceName, IntPtr pEndpointFormat, IntPtr mixFormat);
        [PreserveSig]
        int GetProcessingPeriod(string pszDeviceName, bool bDefault, IntPtr pmftDefaultPeriod, IntPtr pmftMinimumPeriod);
        [PreserveSig]
        int SetProcessingPeriod(string pszDeviceName, IntPtr pmftPeriod);
        [PreserveSig]
        int GetShareMode(string pszDeviceName, IntPtr pMode);
        [PreserveSig]
        int SetShareMode(string pszDeviceName, IntPtr mode);
        [PreserveSig]
        int GetPropertyValue(string pszDeviceName, bool bFxStore, IntPtr key, IntPtr pv);
        [PreserveSig]
        int SetPropertyValue(string pszDeviceName, bool bFxStore, IntPtr key, IntPtr pv);
        [PreserveSig]
        int SetDefaultEndpoint(string pszDeviceName, Role role);
        [PreserveSig]
        int SetEndpointVisibility(string pszDeviceName, bool bVisible);
    }
}
