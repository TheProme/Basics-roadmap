using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace GalleryMVVM.MVVM.Models
{
    public class FolderModel
    {
        public FolderModel()
        {
            if(this is DummyFolder)
            {
                return;
            }
            
        }
        public ObservableCollection<FolderModel> Subfolders { get; private set; } = new ObservableCollection<FolderModel>();

        private string _directoryPath;
        public string DirectoryPath 
        {
            get => _directoryPath;
            set
            {
                _directoryPath = value;
                if(_directoryPath != null)
                {
                    try
                    {
                        if (Directory.GetDirectories(DirectoryPath, "", SearchOption.TopDirectoryOnly).Length > 1)
                        {
                            Subfolders.Add(new DummyFolder("dummy"));
                        }
                    }
                    catch(Exception ex)
                    {

                    }
                }
            }
        }
        public bool IsSelected { get; set; }

        private string _name;
        public string Name
        {
            get => _name ??= Path.GetFileName(DirectoryPath);
            set => _name = value;
        }

        private bool _isExpanded;
        public bool IsExpanded 
        {
            get => _isExpanded;
            set
            {
                _isExpanded = value;
                SetSubfolders();
            }
        }

        public void SetSubfolders()
        {
            Subfolders.Clear();
            Task.Run(() =>
            {
                string[] directories = null;
                try
                {
                    directories = Directory.GetDirectories(DirectoryPath, "", SearchOption.TopDirectoryOnly);
                }
                catch (Exception ex)
                {
                    //usually unauthorized exception
                }
                if (directories != null)
                {
                    foreach (var directory in directories)
                    {
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            Subfolders.Add(new FolderModel { DirectoryPath = directory });
                        });
                    }
                }
            });
        }
        
    }
}
