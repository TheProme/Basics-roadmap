using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace GalleryMVVM.MVVM.Models
{
    public class FolderModel
    {
        public ObservableCollection<FolderModel> Subfolders { get; set; } = new ObservableCollection<FolderModel>();
        public string DirectoryPath { get; set; }
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

        private void SetSubfolders()
        {
            if (Subfolders.Count < 1)
            {
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
}
