using System.Windows;
using System.Windows.Input;

namespace SimplePaint.Events
{
    public class MousePositionToElementEventArgs
    {
        public MouseEventArgs MouseEventArgs { get; set; }
        public Point RelativeToElementPoint { get; set; } 
    }
}
