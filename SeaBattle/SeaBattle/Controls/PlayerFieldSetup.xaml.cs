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
    public partial class PlayerFieldSetup : UserControl
    {
        public SetupFieldViewModel SetupFieldViewModel
        {
            get { return (SetupFieldViewModel)GetValue(SetupFieldViewModelProperty); }
            set { SetValue(SetupFieldViewModelProperty, value); }
        }

        public static readonly DependencyProperty SetupFieldViewModelProperty =
            DependencyProperty.Register("SetupFieldViewModel", typeof(SetupFieldViewModel), typeof(PlayerFieldSetup), new PropertyMetadata(null, SetupViewModelChanged));

        private static void SetupViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PlayerFieldSetup).SetValue(SetupFieldViewModelProperty, e.NewValue);
            (d as PlayerFieldSetup).DataContext = e.NewValue;
        }



        public bool CanOpponentHit
        {
            get { return (bool)GetValue(CanOpponentHitProperty); }
            set { SetValue(CanOpponentHitProperty, value); }
        }

        public static readonly DependencyProperty CanOpponentHitProperty =
            DependencyProperty.Register("CanOpponentHit", typeof(bool), typeof(PlayerFieldSetup), new PropertyMetadata(true));



        public PlayerFieldSetup()
        {
            InitializeComponent();
        }

        private void FieldMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(SetupFieldViewModel != null)
            {
                if (SetupFieldViewModel.IsRemoving)
                {
                    SetupFieldViewModel.RemoveShip(SetupFieldViewModel.MousePosition);
                }
                else
                {
                    SetupFieldViewModel.PlaceShip(SetupFieldViewModel.ShipToPlace);
                }
            }
        }
    }
}
