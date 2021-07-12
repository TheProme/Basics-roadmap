using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GalleryMVVM
{
    /// <summary>
    /// Interaction logic for RatingControl.xaml
    /// </summary>
    public partial class RatingControl : UserControl
    {
        public RatingControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty RatingValueProperty = DependencyProperty.Register("RatingValue", typeof(int),typeof(RatingControl),new PropertyMetadata(0, new PropertyChangedCallback(RatingValueChanged)));

        public int RatingValue
        {
            get
            {

                return (int)GetValue(RatingValueProperty);
            }
            set
            {
                SetValue(RatingValueProperty, value);
            }
        }

        private static void RatingValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            RatingControl uc = sender as RatingControl;
            int ratingValue = (int)e.NewValue;
            UIElementCollection children = uc.ratingPanel.Children;

            ToggleButton button = null;
            for (int i = 0; i < ratingValue; i++)

            {
                button = children[i] as ToggleButton;
                button.IsChecked = true;
            }

            for (int i = ratingValue; i < children.Count; i++)
            {
                button = children[i] as ToggleButton;
                button.IsChecked = false;
            }
        }

        private void RatingButtonClick_Handler(object sender, RoutedEventArgs e)
        {
            ToggleButton button = sender as ToggleButton;
            RatingValue = Int32.Parse((String)button.Tag);
        }
    }
}
