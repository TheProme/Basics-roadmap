using SeaBattle.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SeaBattle.Controls
{
    public class Ship : Control
    {
        static Ship()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Ship), new FrameworkPropertyMetadata(typeof(Ship)));
        }


        public ShipViewModel ShipModel
        {
            get { return (ShipViewModel)GetValue(ShipModelProperty); }
            set { SetValue(ShipModelProperty, value); }
        }

        public static readonly DependencyProperty ShipModelProperty =
            DependencyProperty.Register("ShipModel", typeof(ShipViewModel), typeof(Ship), new PropertyMetadata(null));
    }
}
