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
    public class Field : Control
    {
        static Field()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Field), new FrameworkPropertyMetadata(typeof(Field)));
        }



        public bool CanClick
        {
            get { return (bool)GetValue(CanClickProperty); }
            set { SetValue(CanClickProperty, value); }
        }

        public static readonly DependencyProperty CanClickProperty =
            DependencyProperty.Register("CanClick", typeof(bool), typeof(Field), new PropertyMetadata(null));

        public bool IsPlayerField
        {
            get { return (bool)GetValue(IsPlayerFieldProperty); }
            set { SetValue(IsPlayerFieldProperty, value); }
        }

        public static readonly DependencyProperty IsPlayerFieldProperty =
            DependencyProperty.Register("IsPlayerField", typeof(bool), typeof(Field), new PropertyMetadata(false));


        public ObservableCollection<FieldCellViewModel> FieldCells
        {
            get { return (ObservableCollection<FieldCellViewModel>)GetValue(FieldCellsProperty); }
            set { SetValue(FieldCellsProperty, value); }
        }

        public static readonly DependencyProperty FieldCellsProperty =
            DependencyProperty.Register("FieldCells", typeof(ObservableCollection<FieldCellViewModel>), typeof(Field), new PropertyMetadata(CreateFieldCells(GameRules.FieldSize), FieldCellsChange));

        private static void FieldCellsChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Field;
            control.SetValue(FieldCellsProperty, e.NewValue);
        }

        private static ObservableCollection<FieldCellViewModel> CreateFieldCells(int size)
        {
            ObservableCollection<FieldCellViewModel> cells = new ObservableCollection<FieldCellViewModel>();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    EmptyCellViewModel emptyCell = new EmptyCellViewModel(new Position(i, j));
                    cells.Add(new FieldCellViewModel(emptyCell.Position, emptyCell));
                }
            }
            return cells;
        }

        public ObservableCollection<ShipViewModel> Ships
        {
            get { return (ObservableCollection<ShipViewModel>)GetValue(ShipsProperty); }
            set { SetValue(ShipsProperty, value); }
        }

        public static readonly DependencyProperty ShipsProperty =
            DependencyProperty.Register("Ships", typeof(ObservableCollection<ShipViewModel>), typeof(Field), new PropertyMetadata(null));

        public bool FieldIsReady
        {
            get { return (bool)GetValue(FieldIsReadyProperty); }
            set { SetValue(FieldIsReadyProperty, value); }
        }

        public static readonly DependencyProperty FieldIsReadyProperty =
            DependencyProperty.Register("FieldIsReady", typeof(bool), typeof(Field), new PropertyMetadata(false));


    }
}
