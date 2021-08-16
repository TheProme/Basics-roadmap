using SeaBattle.Extensions;
using SeaBattle.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    public partial class GameField : UserControl
    {
        public SetupFieldViewModel SetupFieldVM { get; set; }
        public GameField()
        {
            SetupFieldVM = new SetupFieldViewModel();
            DataContext = SetupFieldVM;
            InitializeComponent();
        }

        private void FieldMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (SetupFieldVM.IsRemoving)
            {
                SetupFieldVM.Ships.Remove(SetupFieldVM.FieldVM.GetShipByPosition(SetupFieldVM.MousePosition));
            }
            else
            {
                SetupFieldVM.PlaceShip(SetupFieldVM.ShipToPlace);
            }
        }
    }
}
