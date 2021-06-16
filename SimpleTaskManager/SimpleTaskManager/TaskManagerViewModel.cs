using NvAPIWrapper.GPU;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleTaskManager
{
    public class TaskManagerViewModel: BaseViewModel
    {
        private object _locker = new object();

        private ProcessViewModel _selectedProcess;

        public ProcessViewModel SelectedProcess
        {
            get { return _selectedProcess; }
            set { _selectedProcess = value; OnPropertyChanged(); }
        }

        public ObservableCollection<ProcessViewModel> RunningProcesses { get; set; } = new ObservableCollection<ProcessViewModel>();

        private ICommand killProcessCommand;
        public ICommand KillProcessCommand
        {
            get
            {
                return killProcessCommand ??
                  (killProcessCommand = new RelayCommand(obj =>
                  {
                      SelectedProcess.KillProcess();
                  }, obj => obj != null));
            }
        }

        private double _gpuPercentage;

        public double GpuPercentage
        {
            get { return _gpuPercentage; }
            set { _gpuPercentage = value; OnPropertyChanged(); }
        }

        private ushort _cpuPercentage;

        public ushort CpuPercentage
        {
            get { return _cpuPercentage; }
            set { _cpuPercentage = value; OnPropertyChanged(); }
        }

        private double _memoryPercentage;

        public double MemoryPercentage
        {
            get { return _memoryPercentage; }
            set { _memoryPercentage = value; OnPropertyChanged(); }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private class MEMORYSTATUSEX
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


        private void SetPercentageData()
        {
            foreach (var item in PhysicalGPU.GetPhysicalGPUs())
            {
                GpuPercentage = item.UsageInformation.GPU.Percentage;
            }
            MEMORYSTATUSEX memStatus = new MEMORYSTATUSEX();
            if (GlobalMemoryStatusEx(memStatus))
            {
                MemoryPercentage = Math.Round((double)memStatus.ullAvailPhys / memStatus.ullTotalPhys * 100, 2);
            }
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
            foreach (ManagementObject obj in searcher.Get())
            {
                CpuPercentage = (ushort)obj["LoadPercentage"];
            }
        }

        private Thread _processChecker;

        public TaskManagerViewModel()
        {
            _processChecker = new Thread(new ThreadStart(CheckProcesses)) { IsBackground = true };
            _processChecker.Start();
        }

        private void CheckProcesses()
        {
            while(true)
            {
                SetPercentageData();
                Process[] gpuProcesses = PhysicalGPU.GetPhysicalGPUs().FirstOrDefault()?.GetActiveApplications();
                Process[] processes = Process.GetProcesses();
                foreach (var item in processes)
                {
                    var equalGpuProcess = gpuProcesses.FirstOrDefault(x => x.Id == item.Id);
                    ProcessViewModel existingProcess;
                    lock (_locker)
                    {
                        existingProcess = RunningProcesses.FirstOrDefault(proc => proc.Id == item.Id);
                    }
                    if (existingProcess == null)
                    {
                        try
                        {
                            item.EnableRaisingEvents = true;
                            item.Exited += Process_Exited;
                            App.Current.Dispatcher.Invoke(() =>
                            {
                                RunningProcesses.Add(new ProcessViewModel(item));
                            });
                            
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"{item.ProcessName} denied request for enabling events!");
                        }
                    }
                    else
                    {
                        existingProcess.Refresh(equalGpuProcess == null ? false : true);
                    }
                }
                Thread.Sleep(1000);
            }
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            var exitedProcess = sender as Process;
            lock (_locker)
            {
                var existingProcess = RunningProcesses.FirstOrDefault(proc => proc.Id == exitedProcess.Id);
                App.Current.Dispatcher.Invoke(() =>
                {
                    RunningProcesses.Remove(existingProcess);
                });
            }
            Console.WriteLine($"{exitedProcess?.ProcessName} exited!");
        }
    }
}
