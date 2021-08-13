using SeaBattle.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SeaBattle.Controls
{
    public class FieldCell : Control
    {
        static FieldCell()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FieldCell), new FrameworkPropertyMetadata(typeof(FieldCell)));
        }
        public FieldCellViewModel FieldCellModel
        {
            get { return (FieldCellViewModel)GetValue(FieldCellModelProperty); }
            set { SetValue(FieldCellModelProperty, value); }
        }

        public static readonly DependencyProperty FieldCellModelProperty =
            DependencyProperty.Register("FieldCellModel", typeof(FieldCellViewModel), typeof(FieldCell), new PropertyMetadata(null));
    }
}
