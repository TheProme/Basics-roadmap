using SimplePaint.Events;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace SimplePaint.Converters
{
    public class MousePositionToElementConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var mouseEventArgs = value as MouseEventArgs;
            var relativePoint = mouseEventArgs.GetPosition(parameter as FrameworkElement ?? throw new ArgumentException());
            return new MousePositionToElementEventArgs() { MouseEventArgs = mouseEventArgs, RelativeToElementPoint = relativePoint };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
