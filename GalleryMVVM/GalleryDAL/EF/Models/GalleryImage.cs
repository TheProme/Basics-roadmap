using System;
using System.Collections.Generic;
using System.Text;

namespace GalleryDAL.EF.Models
{
    public class GalleryImage
    {
		public int ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int Rating { get; set; }
		public string Path { get; set; }
	}
}
