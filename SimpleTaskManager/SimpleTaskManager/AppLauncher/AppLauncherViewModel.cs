using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace SimpleTaskManager.AppLauncher
{
    public class AppLauncherViewModel : BaseViewModel
    {
        public delegate string ShowFolder(string path);
        public event ShowFolder ShowApplicationFolderEvent;

        private readonly string STEAMAPP_KEY = @"SOFTWARE\Valve\Steam\Apps";
        private readonly string REGISTRY_KEY = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
        public ObservableCollection<ApplicationViewModel> ApplicationViews { get; set; } = new ObservableCollection<ApplicationViewModel>();

        public ICollectionView FilteredAppViews
        {
            get { return CollectionViewSource.GetDefaultView(ApplicationViews); }
        }

        private ICommand runApplicationCommand;
        public ICommand RunApplicationCommand
        {
            get
            {
                return runApplicationCommand ??
                  (runApplicationCommand = new RelayCommand(obj => RunApp(obj), obj => obj != null));
            }
        }

        private string _filterText;

        public string FilterText
        {
            get { return _filterText; }
            set 
            { 
                _filterText = value; 
                OnPropertyChanged();
                FilteredAppViews.Filter = app => {
                    var item = app as ApplicationViewModel;
                    if(item.Name.ToLower().Contains(_filterText.ToLower()))
                    {
                        return true;
                    }
                    return false;
                };
            }
        }


        public AppLauncherViewModel()
        {
            SetApplicationsFromRegistry();
        }


        private void RunApp(object application)
        {
            if (application is SteamAppViewModel)
            {
                Process.Start($"steam://rungameid/{(application as SteamAppViewModel).GameID}");
            }
            else
            {
                string selectedFile = ShowApplicationFolderEvent?.Invoke((application as ApplicationViewModel).Path);
                if (!String.IsNullOrEmpty(selectedFile))
                {
                    Process.Start(selectedFile);
                }
            }

        }


        private void ReadAppsFromRegistry(RegistryHive registryHive, RegistryView registryView)
        {
            RegistryKey registryKey = RegistryKey.OpenBaseKey(registryHive, registryView);
            using (RegistryKey key = registryKey.OpenSubKey(REGISTRY_KEY))
            {
                if (key != null)
                {
                    foreach (string subkey_name in key.GetSubKeyNames())
                    {
                        using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                        {
                            if (subkey.GetValue("DisplayName") != null && subkey.GetValue("InstallLocation") != null && !String.IsNullOrEmpty(subkey.GetValue("InstallLocation").ToString()))
                            {
                                ApplicationViews.Add(new ApplicationViewModel(subkey));
                            }
                        }
                    }
                }
            }
        }

        private void SetApplicationsFromRegistry()
        {
            ReadAppsFromRegistry(RegistryHive.LocalMachine, RegistryView.Registry64);
            ReadAppsFromRegistry(RegistryHive.CurrentUser, RegistryView.Registry64);
            ReadAppsFromRegistry(RegistryHive.LocalMachine, RegistryView.Registry32);
            ReadAppsFromRegistry(RegistryHive.CurrentUser, RegistryView.Registry32);
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(STEAMAPP_KEY))
            {
                if(key != null)
                {
                    foreach (string subkey_name in key.GetSubKeyNames())
                    {
                        using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                        {
                            if (subkey.GetValue("Name") != null)
                            {
                                string gameName = subkey.GetValue("Name").ToString();
                                if (ApplicationViews.Any(app => app.Name == gameName))
                                {
                                    int gameID = Convert.ToInt32(subkey.Name.Substring(subkey.Name.LastIndexOf('\\') + 1));
                                    var existingItem = ApplicationViews.First(app => app.Name == gameName);
                                    ApplicationViews[ApplicationViews.IndexOf(existingItem)] = new SteamAppViewModel() { Name = existingItem.Name, IconBmp = existingItem.IconBmp, Path = existingItem.Path, GameID = gameID };
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
