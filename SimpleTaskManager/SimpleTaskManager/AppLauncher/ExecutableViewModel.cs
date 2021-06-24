using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace SimpleTaskManager.AppLauncher
{
    public class ExecutableViewModel : BaseViewModel
    {
		private BitmapSource _icon;

		public BitmapSource Icon
		{
			get { return _icon; }
			set { _icon = value; OnPropertyChanged(); }
		}

		private string _name;

		public string Name
		{
			get { return _name; }
			set { _name = value; OnPropertyChanged(); }
		}

		private string _fullPath;

		public string FullPath
		{
			get { return _fullPath; }
			set { _fullPath = value; OnPropertyChanged(); }
		}

		public ExecutableViewModel(string path)
		{
			SetIcon(path);
			FullPath = path;
			Name = Path.GetFileNameWithoutExtension(path);
		}

		private void SetIcon(string path)
		{
			using (Icon ico = System.Drawing.Icon.ExtractAssociatedIcon(path))
			{
				Icon = Imaging.CreateBitmapSourceFromHIcon(ico.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
			}
		}
	}
}
