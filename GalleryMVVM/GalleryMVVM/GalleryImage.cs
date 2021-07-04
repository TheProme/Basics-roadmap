using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryMVVM
{
    public class GalleryImage : BaseViewModel
    {
		private int _id;

		public int ID
		{
			get { return _id; }
			set { _id = value; }
		}

		private string _name;

		public string Name
		{
			get { return _name; }
			set { _name = value; OnPropertyChanged(); }
		}
		private string _description;

		public string Description
		{
			get { return _description; }
			set { _description = value; OnPropertyChanged(); }
		}

		private int _rating;

		public int Rating
		{
			get { return _rating; }
			set { _rating = value; OnPropertyChanged(); }
		}

		private string _path;

		public string Path
		{
			get { return _path; }
			set { _path = value; OnPropertyChanged(); }
		}

		private bool _isChangable;

		public bool IsChangable
		{
			get { return _isChangable; }
			set { _isChangable = value; OnPropertyChanged(); }
		}

	}
}
