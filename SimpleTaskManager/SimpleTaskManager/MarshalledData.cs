using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTaskManager
{
    internal class MarshalledData
    {
        private Process _process;
        #region RAM Marshalling
        private struct PROCESS_MEMORY_COUNTERS_EX
        {
            public uint cb;
            public uint PageFaultCount;
            public UIntPtr PeakWorkingSetSize;
            public UIntPtr WorkingSetSize;
            public UIntPtr QuotaPeakPagedPoolUsage;
            public UIntPtr QuotaPagedPoolUsage;
            public UIntPtr QuotaPeakNonPagedPoolUsage;
            public UIntPtr QuotaNonPagedPoolUsage;
            public UIntPtr PagefileUsage;
            public UIntPtr PeakPagefileUsage;
            public UIntPtr PrivateUsage;
        }
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("psapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool GetProcessMemoryInfo(IntPtr processHandle, ref PROCESS_MEMORY_COUNTERS_EX pmc, uint pmcSizeBytes);


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct MEMORYSTATUSEX
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
        }


        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool GlobalMemoryStatusEx(ref MEMORYSTATUSEX lpBuffer);
        #endregion
        #region CPU Marshalling
        private FILETIME ftime, fsys, fuser;
        private ulong lastCPU, lastSysCPU, lastUserCPU;
        private uint numProcessors;

        [StructLayout(LayoutKind.Sequential)]
        internal struct SYSTEM_INFO
        {
            internal ushort wProcessorArchitecture;
            internal ushort wReserved;
            internal uint dwPageSize;
            internal IntPtr lpMinimumApplicationAddress;
            internal IntPtr lpMaximumApplicationAddress;
            internal IntPtr dwActiveProcessorMask;
            internal uint dwNumberOfProcessors;
            internal uint dwProcessorType;
            internal uint dwAllocationGranularity;
            internal ushort wProcessorLevel;
            internal ushort wProcessorRevision;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern void GetSystemInfo(out SYSTEM_INFO sysInfo);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern void GetSystemTimeAsFileTime(out FILETIME ftime);

        private uint GetNumberOfProcessors()
        {
            SYSTEM_INFO sys = new SYSTEM_INFO();
            GetSystemInfo(out sys);
            return sys.dwNumberOfProcessors;
        }


        private struct ProcessTimes
        {
            public long RawCreationTime;
            public long RawExitTime;
            public long RawKernelTime;
            public long RawUserTime;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetProcessTimes(IntPtr hProcess, out FILETIME lpCreationTime, out FILETIME lpExitTime, out FILETIME lpKernelTime, out FILETIME lpUserTime);

        #endregion
        public MarshalledData(Process process)
        {
            _process = process;
            InitProcessInfo(_process);
        }

        private void InitProcessInfo(Process process)
        {
            FILETIME ftime, fsys, fuser;
            numProcessors = GetNumberOfProcessors();

            GetSystemTimeAsFileTime(out ftime);
            lastCPU = (ulong)ftime.dwLowDateTime;

            GetProcessTimes(process.Handle, out ftime, out ftime, out fsys, out fuser);
            lastSysCPU = (ulong)fsys.dwLowDateTime;
            lastUserCPU = (ulong)fuser.dwLowDateTime;
        }

        public double GetProcessCpuUsage()
        {
            FILETIME ftime, fsys, fuser;
            ulong now, sys, user;
            double percent;

            GetSystemTimeAsFileTime(out ftime);
            now = (ulong)ftime.dwLowDateTime;

            GetProcessTimes(_process.Handle, out ftime, out ftime, out fsys, out fuser);
            sys = (ulong)fsys.dwLowDateTime;
            user = (ulong)fuser.dwLowDateTime;

            percent = (sys - lastSysCPU) + (user - lastUserCPU);
            percent /= (now - lastCPU);
            percent /= numProcessors;
            lastCPU = now;
            lastUserCPU = user;
            lastSysCPU = sys;

            return Math.Round(percent * 100,1);
        }

        public long GetRamUsage()
        {
            long usage = 0;
            PROCESS_MEMORY_COUNTERS_EX pmc = new PROCESS_MEMORY_COUNTERS_EX() { cb = (uint)Marshal.SizeOf(typeof(PROCESS_MEMORY_COUNTERS_EX)) };
            if (GetProcessMemoryInfo(_process.Handle, ref pmc, pmc.cb))
            {
                usage = (long)pmc.PrivateUsage;
            }
            return usage;
        }

        public static double GetTotalMemoryLoad()
        {
            double memoryLoad = 0;
            MEMORYSTATUSEX memStatus = new MEMORYSTATUSEX() { dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX)) };
            if (GlobalMemoryStatusEx(ref memStatus))
            {
                memoryLoad = (double)memStatus.dwMemoryLoad;
            }
            return memoryLoad;
        }

    }
}
