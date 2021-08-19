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

        public int Size
        {
            get { return (int)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register("Size", typeof(int), typeof(Field), new PropertyMetadata(GameRules.FieldSize, SizeChangedCallback, SizeCoerceValue));


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

        private static object SizeCoerceValue(DependencyObject d, object baseValue)
        {
            if ((int)baseValue % 2 == 0 && (int)baseValue > 0)
            {
                return baseValue;
            }
            return 10;
        }

        private static void SizeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Field;
            control.SetValue(SizeProperty, e.NewValue);
            //TODO: update FieldCells + Ships. Правильно подвязать работу с Байндингами
            //control.FieldCells = CreateFieldCells((int)e.NewValue);
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
