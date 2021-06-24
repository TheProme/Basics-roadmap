using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTaskManager.AppLauncher
{
    public class ApplicationFolderViewModel : BaseViewModel
    {
        public ObservableCollection<ExecutableViewModel> ExecutableViews { get; set; } = new ObservableCollection<ExecutableViewModel>();

        private ExecutableViewModel _selectedExecutable;

        public ExecutableViewModel SelectedExecutable
        {
            get { return _selectedExecutable; }
            set { _selectedExecutable = value; OnPropertyChanged(); }
        }

        public ApplicationFolderViewModel(List<string> filePaths)
        {
            foreach (var path in filePaths)
            {
                ExecutableViews.Add(new ExecutableViewModel(path));
            }
        }
    }
}
