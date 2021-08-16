using SeaBattle.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SeaBattle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public FieldViewModel FieldView1 { get; set; }
        public FieldViewModel FieldView2 { get; set; }
        public ObservableCollection<ShipViewModel> Ships1 { get; set; } = new ObservableCollection<ShipViewModel>();
        public ObservableCollection<ShipViewModel> Ships2 { get; set; } = new ObservableCollection<ShipViewModel>();
        public ShipViewModel ShipViewModel1 { get; set; } = new ShipViewModel(Orientation.Horizontal, ShipSize.Medium, new Extensions.Position(0, 2));
        public ShipViewModel ShipViewModel2 { get; set; } = new ShipViewModel(Orientation.Vertical, ShipSize.Large, new Extensions.Position(2, 5));
        public ShipViewModel ShipViewModel3 { get; set; } = new ShipViewModel(Orientation.Horizontal, ShipSize.Small, new Extensions.Position(4, 2));
        public ShipViewModel ShipViewModel4 { get; set; } = new ShipViewModel(Orientation.Horizontal, ShipSize.Tiny, new Extensions.Position(0, 0));
        public MainWindow()
        {
            Ships1.Add(new ShipViewModel(Orientation.Horizontal, ShipSize.Medium, new Extensions.Position(0, 2)));
            Ships1.Add(new ShipViewModel(Orientation.Vertical, ShipSize.Large, new Extensions.Position(2, 5)));
            Ships1.Add(new ShipViewModel(Orientation.Horizontal, ShipSize.Small, new Extensions.Position(4, 2)));
            Ships1.Add(new ShipViewModel(Orientation.Horizontal, ShipSize.Tiny, new Extensions.Position(0, 0)));

            Ships2.Add(new ShipViewModel(Orientation.Horizontal, ShipSize.Medium, new Extensions.Position(0, 2)));
            Ships2.Add(new ShipViewModel(Orientation.Vertical, ShipSize.Large, new Extensions.Position(2, 5)));
            Ships2.Add(new ShipViewModel(Orientation.Horizontal, ShipSize.Small, new Extensions.Position(4, 2)));
            Ships2.Add(new ShipViewModel(Orientation.Horizontal, ShipSize.Tiny, new Extensions.Position(0, 0)));

            FieldView1 = new FieldViewModel(Ships1, 10, true);
            FieldView2 = new FieldViewModel(Ships2, 10);
            this.DataContext = this;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Ships1.Add(new ShipViewModel(Orientation.Horizontal, ShipSize.Tiny, new Extensions.Position(9, 0)));
            Ships2.Add(new ShipViewModel(Orientation.Horizontal, ShipSize.Tiny, new Extensions.Position(8, 4)));
        }
    }
}
