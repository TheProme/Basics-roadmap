using System.ComponentModel;
using System.Diagnostics;

namespace SimpleTaskManager
{
    public class RunningProcess : INotifyPropertyChanged
    {
        private Process _process;

        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged("Id"); }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }

        private long _workingSet;

        public long WorkingSet
        {
            get { return _workingSet; }
            set { _workingSet = value; OnPropertyChanged("WorkingSet"); }
        }

        private bool _isUsingGpu;

        public bool IsUsingGpu
        {
            get { return _isUsingGpu; }
            set { _isUsingGpu = value; OnPropertyChanged("IsUsingGpu"); }
        }


        private double _oldCpuUsage = 0;

        private double _cpuUsage;

        public double CpuUsage
        {
            get { return _cpuUsage; }
            set { _cpuUsage = value; OnPropertyChanged("CpuUsage"); }
        }

        public RunningProcess(Process process)
        {
            _process = process;
            _oldCpuUsage = _process.TotalProcessorTime.TotalMilliseconds;
            SetInfoFromProcess(_process);
        }

        public void Refresh(bool usingGpu)
        {
            SetInfoFromProcess(_process);
            IsUsingGpu = usingGpu;
        }

        private void UpdateCpuUsage()
        {
            CpuUsage = (_process.TotalProcessorTime.TotalMilliseconds - _oldCpuUsage) / 100;
            _oldCpuUsage = _process.TotalProcessorTime.TotalMilliseconds;
        }


        private void SetInfoFromProcess(Process process)
        {
            Id = process.Id;
            Name = process.ProcessName;
            WorkingSet = process.WorkingSet64;
            UpdateCpuUsage();
        }


        public void KillProcess()
        { 
            _process.Kill();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
