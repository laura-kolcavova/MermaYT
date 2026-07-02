using Microsoft.UI;
using System;
using System.Runtime.InteropServices;

namespace MermaYT.WinUi.Services;

internal static class IconService
{
    public static IconId GetApplicationIconId()
    {
        // Application resource ID assigned by Visual Studio to .NET applications
        // https://devblogs.microsoft.com/oldnewthing/20250423-00/?p=111106
        IntPtr iconResourceId = new(32512);

        IntPtr hModule = NativeMethods.GetModuleHandle(null);

        if (hModule == IntPtr.Zero)
        {
            return default;
        }

        IntPtr hIcon = NativeMethods.LoadIcon(
            hModule,
            iconResourceId);

        if (hIcon == IntPtr.Zero)
        {
            return default;
        }

        return Win32Interop.GetIconIdFromIcon(hIcon);
    }

    private static class NativeMethods
    {
        [DllImport("kernel32.dll", EntryPoint = "GetModuleHandle", CharSet = CharSet.Unicode)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        public static extern IntPtr GetModuleHandle(string? lpModuleName);

        [DllImport("user32.dll", EntryPoint = "LoadIconW")]
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        public static extern IntPtr LoadIcon(IntPtr hModule, IntPtr lpIconName);
    }
}
