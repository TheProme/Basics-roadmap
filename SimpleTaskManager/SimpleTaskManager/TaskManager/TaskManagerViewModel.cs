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

        private ObservableCollection<ProcessViewModel> _selectedProcesses = new ObservableCollection<ProcessViewModel>();

        public ObservableCollection<ProcessViewModel> SelectedProcesses
        {
            get { return _selectedProcesses; }
            set { _selectedProcesses = value; OnPropertyChanged(); }
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
                      if(SelectedProcesses.Count > 1)
                      {
                          foreach (var item in SelectedProcesses)
                          {
                              item.KillProcess();
                          }
                      }
                      else
                      {
                          SelectedProcess.KillProcess();
                      }
                  }, obj => obj != null));
            }
        }

        private double _gpuPercentage;

        public double GpuPercentage
        {
            get { return _gpuPercentage; }
            set { _gpuPercentage = value; OnPropertyChanged(); }
        }

        private ulong _cpuPercentage;

        public ulong CpuPercentage
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

        

        private void SetPercentageData()
        {
            foreach (var item in PhysicalGPU.GetPhysicalGPUs())
            {
                GpuPercentage = item.UsageInformation.GPU.Percentage;
            }
            MemoryPercentage = MarshalledData.GetTotalMemoryLoad();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PerfFormattedData_PerfOS_Processor");
            foreach (ManagementObject obj in searcher.Get())
            {
                CpuPercentage = (ulong)obj["PercentProcessorTime"];
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
                try
                {
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
                }
                catch(Exception ex)
                {

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
