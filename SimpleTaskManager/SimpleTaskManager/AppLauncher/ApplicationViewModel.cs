using Microsoft.Win32;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace SimpleTaskManager.AppLauncher
{
    public class ApplicationViewModel: BaseViewModel
    {
        public struct EmbeddedIconInfo
        {
            public string FileName;
            public int IconIndex;
        }
        [DllImport("shell32.dll", EntryPoint = "ExtractIconA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern IntPtr ExtractIcon(int hInst, string lpszExeFileName, int nIconIndex);


        private RegistryKey _appRegistryKey;

        public ApplicationViewModel()
        {

        }

        public ApplicationViewModel(RegistryKey registryKey)
        {
            _appRegistryKey = registryKey;
            Name = _appRegistryKey.GetValue("DisplayName").ToString();
            Path = _appRegistryKey.GetValue("InstallLocation").ToString();
            if (_appRegistryKey.GetValue("DisplayIcon") != null)
            {
                IconBmp = ExtractIconFromFile(_appRegistryKey.GetValue("DisplayIcon").ToString());
            }
            else
            {
                IconBmp = Imaging.CreateBitmapSourceFromHIcon(DefaultIcons.ApplicationIconLarge.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
        }



        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        private string _path;

        public string Path
        {
            get { return _path; }
            set { _path = value; OnPropertyChanged(); }
        }

        private BitmapSource _iconBmp;

        public BitmapSource IconBmp
        {
            get { return _iconBmp; }
            set { _iconBmp = value; OnPropertyChanged(); }
        }

        private EmbeddedIconInfo GetEmbeddedIconInfo(string iconPath)
        {
            EmbeddedIconInfo embeddedIcon = new EmbeddedIconInfo();
            if (!String.IsNullOrEmpty(iconPath))
            {
                string fileName = "";

                int iconIndex = 0;

                int commaIndex = iconPath.IndexOf(",");
                if (commaIndex > 0)
                {
                    fileName = iconPath.Substring(0, commaIndex);
                    iconIndex = int.Parse(iconPath.Substring(commaIndex + 1)) < 0 ? 0 : int.Parse(iconPath.Substring(commaIndex + 1));
                }
                else
                    fileName = iconPath;


                embeddedIcon.FileName = fileName;
                embeddedIcon.IconIndex = iconIndex;
            }
            return embeddedIcon;
        }

        private BitmapSource ExtractIconFromFile(string filePath)
        {
            try
            {
                if(filePath.Contains(".ico"))
                {
                    using (Icon ico = Icon.ExtractAssociatedIcon(filePath))
                    {
                        return Imaging.CreateBitmapSourceFromHIcon(ico.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    }
                }
                else
                {
                    EmbeddedIconInfo embeddedIcon = GetEmbeddedIconInfo(filePath);

                    IntPtr lIcon = ExtractIcon(0, embeddedIcon.FileName, embeddedIcon.IconIndex);

                    return Imaging.CreateBitmapSourceFromHIcon(lIcon, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                }
            }
            catch (Exception exc)
            {
                return null;
            }
        }

    }
}
