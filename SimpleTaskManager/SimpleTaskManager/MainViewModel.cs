﻿using SimpleTaskManager.AppLauncher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTaskManager
{
    public class MainViewModel : BaseViewModel
    {
        public TaskManagerViewModel TaskManagerVM { get; private set; } = new TaskManagerViewModel();
        public AppLauncherViewModel AppLauncherVM { get; private set; } = new AppLauncherViewModel();

        public MainViewModel()
        {
            AppLauncherVM.ShowApplicationFolderEvent += ShowApplicationFolder_Handler;
        }

        private string ShowApplicationFolder_Handler(string path)
        {
            string filePath = string.Empty;
            ApplicationFolderWindow folderWindow = new ApplicationFolderWindow(path);
            folderWindow.ShowDialog();
            if (folderWindow.DialogResult == true)
            {
                filePath = folderWindow.FilePath;
            }
            return filePath;
        }
    }
}
