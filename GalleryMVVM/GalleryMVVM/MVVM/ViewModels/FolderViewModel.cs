using GalleryMVVM.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GalleryMVVM.MVVM.ViewModels
{
    public class FolderViewModel : BaseViewModel
    {
        public delegate void FolderChangedDelegateAsync(List<string> imagesInFolder);
        public event FolderChangedDelegateAsync FolderChanged;

        public ObservableCollection<FolderModel> Folders { get; set; } = new ObservableCollection<FolderModel>();

        private FolderModel _currentFolder;

        public FolderModel CurrentFolder
        {
            get => _currentFolder;
            set 
            { 
                _currentFolder = value;
                OnPropertyChanged();
                if(value != null)
                {
                    FolderChanged?.Invoke(GetImagesFromPath(value.DirectoryPath).Result);
                }
            }
        }

        public bool IsSelected
        {
            get => CurrentFolder.IsSelected;
            set
            {
                CurrentFolder.IsSelected = value;
                OnPropertyChanged();
            }
        }

        public bool IsExpanded
        {
            get => CurrentFolder.IsExpanded;
            set
            {
                CurrentFolder.IsExpanded = value;
                OnPropertyChanged();
            }
        }

        private Task<List<string>> GetImagesFromPath(string path)
        {
            return Task.Run(() =>
            {
                string[] patterns = new string[] { "*.jpg", "*.jpeg", "*.png", "*.bmp", "*.ico" };
                List<string> imageStrings = new List<string>();
                foreach (var pattern in patterns)
                {
                    try
                    {
                        foreach (var imagePath in Directory.GetFiles(path, pattern, SearchOption.TopDirectoryOnly))
                        {
                            if (!imageStrings.Exists(str => str == imagePath))
                            {
                                imageStrings.Add(imagePath);
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        //usually access denied
                    }
                }
                return imageStrings;
            });
        }

        private ICommand _deselectFolder;
        public ICommand DeselectFolder
        {
            get => _deselectFolder ?? (_deselectFolder = new ParametrizedCommand(obj =>
            {
                //TODO deselect a ****ing folder...
            }));
        }
        public FolderViewModel()
        {
            InitializeRootFolders();
        }

        private void InitializeRootFolders()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            Task.Run(() =>
            {
                foreach (var drive in allDrives)
                {
                    if (drive.IsReady)
                    {
                        try
                        {
                            Folders.Add(new FolderModel { DirectoryPath = Directory.GetDirectoryRoot(drive.RootDirectory.FullName), Name = drive.Name });
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            });
        }
    }
}
