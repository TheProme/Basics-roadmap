using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace SimpleTaskManager
{
    public class ProcessViewModel : BaseViewModel
    {
        private Process _process;

        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        private long _workingSet;

        public long WorkingSet
        {
            get { return _workingSet; }
            set { _workingSet = value; OnPropertyChanged(); }
        }

        private bool _isUsingGpu;

        public bool IsUsingGpu
        {
            get { return _isUsingGpu; }
            set { _isUsingGpu = value; OnPropertyChanged(); }
        }

        private double _cpuUsage;

        public double CpuUsage
        {
            get { return _cpuUsage; }
            set { _cpuUsage = value; OnPropertyChanged(); }
        }

        private BitmapSource _iconImage;

        public BitmapSource IconImage
        {
            get { return _iconImage; }
            set { _iconImage = value; OnPropertyChanged(); }
        }

        private MarshalledData marshData;
        public ProcessViewModel(Process process)
        {
            _process = process;
            marshData = new MarshalledData(_process);
            try
            {
                using (Icon ico = Icon.ExtractAssociatedIcon(_process.MainModule.FileName))
                {
                    IconImage = Imaging.CreateBitmapSourceFromHIcon(ico.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                }
            }
            catch(Exception ex)
            {

            }
            SetInfoFromProcess(_process);
        }

        

        public void Refresh(bool usingGpu)
        {
            SetInfoFromProcess(_process);
            IsUsingGpu = usingGpu;
        }

        private void UpdateCpuUsage()
        {
            CpuUsage = marshData.GetProcessCpuUsage();
        }


        private void SetInfoFromProcess(Process process)
        {
            Id = process.Id;
            Name = process.ProcessName;

            WorkingSet = marshData.GetRamUsage();
            UpdateCpuUsage();
        }


        public void KillProcess()
        { 
            _process.Kill();
        }
    }
}
