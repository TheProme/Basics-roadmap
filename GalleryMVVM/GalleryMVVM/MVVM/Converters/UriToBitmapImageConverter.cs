using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace GalleryMVVM
{
    public class UriToBitmapImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bmp = new BitmapImage
            {
                CacheOption = BitmapCacheOption.OnDemand,
                CreateOptions = BitmapCreateOptions.DelayCreation
            };
            bmp.BeginInit();
            bmp.DecodePixelWidth = 160;
            bmp.UriSource = new Uri((string)value);
            bmp.EndInit();
            return bmp;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new BitmapImage(new Uri(@"pack://application:,,,/Icons/placeholder.png"));
        }
    }
}
