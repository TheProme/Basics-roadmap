using NvAPIWrapper.GPU;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleTaskManager
{
    public class ProcessViewModel: INotifyPropertyChanged
    {
        private object _locker = new object();
        public RunningProcess SelectedProcess { get; set; }

        public ObservableCollection<RunningProcess> RunningProcesses { get; set; } = new ObservableCollection<RunningProcess>();

        private RelayCommand killProcessCommand;
        public RelayCommand KillProcessCommand
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

        private int _gpuPercentage;

        public int GpuPercentage
        {
            get { return _gpuPercentage; }
            set { _gpuPercentage = value; OnPropertyChanged("GpuPercentage"); }
        }


        private void SetGpuData()
        {
            foreach (var item in PhysicalGPU.GetPhysicalGPUs())
            {
                GpuPercentage = item.UsageInformation.GPU.Percentage;
            }
        }

        private Thread _processChecker;

        public ProcessViewModel()
        {
            _processChecker = new Thread(new ThreadStart(CheckProcesses)) { IsBackground = true };
            _processChecker.Start();
        }

        private void CheckProcesses()
        {
            while(true)
            {
                SetGpuData();
                Process[] gpuProcesses = PhysicalGPU.GetPhysicalGPUs().FirstOrDefault()?.GetActiveApplications();
                Process[] processes = Process.GetProcesses();
                foreach (var item in processes)
                {
                    var equalGpuProcess = gpuProcesses.FirstOrDefault(x => x.Id == item.Id);
                    RunningProcess existingProcess;
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
                                RunningProcesses.Add(new RunningProcess(item));
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

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
