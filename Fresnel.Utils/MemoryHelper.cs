using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Envivo.Fresnel.Utils
{
    /// <summary>
    ///
    /// </summary>
    public class MemoryHelper
    {
        private long _previousBytesInUse = GC.GetTotalMemory(true);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern bool SetProcessWorkingSetSize(IntPtr handle, int minSize, int maxSize);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MEMORYSTATUSEX
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public ulong ullTotalPhys;
            public ulong ullAvailPhys;
            public ulong ullTotalPageFile;
            public ulong ullAvailPageFile;
            public ulong ullTotalVirtual;
            public ulong ullAvailVirtual;
            public ulong ullAvailExtendedVirtual;

            public MEMORYSTATUSEX()
            {
                this.dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
            }
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer);

        public MEMORYSTATUSEX MemoryStatus { get; private set; }

        public MemoryHelper()
        {
            this.MemoryStatus = new MEMORYSTATUSEX();
            this.UpdateMemoryStatus();
        }

        public void UpdateMemoryStatus()
        {
            GlobalMemoryStatusEx(MemoryStatus);
        }

        /// <summary>
        /// Minimises the process' working set memory.
        /// </summary>
        /// <remarks>Use this to manage the application's memory usage</remarks>
        public static void MinimiseWorkingSet()
        {
            SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
        }

        public void CollectMemory()
        {
            MinimiseWorkingSet();

            if (System.Diagnostics.Debugger.IsAttached)
            {
                // This allows us to monitor memory usage for the My.Application.
                // NB: Each item in the MessageListView consumes about 1Kb, so that may be ignored.

                for (int i = 1; i <= 3; i++)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }

                long bytesInUse = GC.GetTotalMemory(true);
                long difference = bytesInUse - _previousBytesInUse;
                _previousBytesInUse = bytesInUse;

                var msg = string.Format("Current Memory: {0:#,###,##0} bytes ({1:+#,###,##0;-#,###,##0;0})", bytesInUse, difference);
                System.Diagnostics.Debug.WriteLine(msg);
            }
        }
    }
}