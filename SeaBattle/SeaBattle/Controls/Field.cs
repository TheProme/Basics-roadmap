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
        //TODO Clickable Grid control? с подложкой существующего Grid
        static Field()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Field), new FrameworkPropertyMetadata(typeof(Field)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //ResetField(Size);
        }



        public FieldViewModel FieldModel
        {
            get { return (FieldViewModel)GetValue(FieldModelProperty); }
            set { SetValue(FieldModelProperty, value); }
        }

        public static readonly DependencyProperty FieldModelProperty =
            DependencyProperty.Register("FieldModel", typeof(FieldViewModel), typeof(Field), new PropertyMetadata(null));



        //public ObservableCollection<FieldCellViewModel> FieldCells { get; set; } = new ObservableCollection<FieldCellViewModel>();

        //public bool IsPlayerField
        //{
        //    get { return (bool)GetValue(IsPlayerFieldProperty); }
        //    set { SetValue(IsPlayerFieldProperty, value); }
        //}

        //public static readonly DependencyProperty IsPlayerFieldProperty =
        //    DependencyProperty.Register("IsPlayerField", typeof(bool), typeof(Field), new PropertyMetadata(false));

        //public int Size
        //{
        //    get { return (int)GetValue(SizeProperty); }
        //    set { SetValue(SizeProperty, value); }
        //}

        //public static readonly DependencyProperty SizeProperty =
        //    DependencyProperty.Register("Size", typeof(int), typeof(Field), new PropertyMetadata(10, SizeChangedCallback, SizeCoerceValue));

        //private static object SizeCoerceValue(DependencyObject d, object baseValue)
        //{
        //    if ((int)baseValue % 2 == 0 && (int)baseValue > 0)
        //    {
        //        return baseValue;
        //    }
        //    return 10;
        //}

        //private static void SizeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var control = d as Field;
        //    control.SetValue(SizeProperty, e.NewValue);
        //    control.FieldModel.Size = (int)e.NewValue;
        //}

        //public ObservableCollection<ShipViewModel> Ships
        //{
        //    get { return (ObservableCollection<ShipViewModel>)GetValue(ShipsProperty); }
        //    set { SetValue(ShipsProperty, value); }
        //}

        //public static readonly DependencyProperty ShipsProperty =
        //    DependencyProperty.Register("Ships", typeof(ObservableCollection<ShipViewModel>), typeof(Field), new PropertyMetadata(null, ShipsSet));

        //private static void ShipsSet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var control = (d as Field);
        //    control.SetValue(ShipsProperty, e.NewValue);
        //    control.ResetField(control.Size);
        //    foreach (var ship in e.NewValue as ObservableCollection<ShipViewModel>)
        //    {
        //        ship.ShipSetEvent += control.ShipSetHandler;
        //        foreach (var block in ship.ShipBlocks)
        //        {
        //            var existingCell = control.FieldCells.FirstOrDefault(cell => cell.Position == block.Position);
        //            var index = control.FieldCells.IndexOf(existingCell);
        //            control.FieldCells.Remove(existingCell);
        //            control.FieldCells.Insert(index, new FieldCellViewModel(block.Position, block));
        //        }

        //    }
        //}

        //private void ShipSetHandler()
        //{
        //    if(Ships.All(ship => ship.IsSet))
        //    {
        //        FieldIsReady = true;
        //        foreach (var ship in Ships)
        //        {
        //            ship.ShipSetEvent -= ShipSetHandler;
        //        }
        //    }
        //}

        //public bool FieldIsReady
        //{
        //    get { return (bool)GetValue(FieldIsReadyProperty); }
        //    private set { SetValue(FieldIsReadyProperty, value); }
        //}

        //public static readonly DependencyProperty FieldIsReadyProperty =
        //    DependencyProperty.Register("FieldIsReady", typeof(bool), typeof(Field), new PropertyMetadata(false));



        //private void ResetField(int size)
        //{
        //    if (FieldCells.Count > 0)
        //    {
        //        FieldCells.Clear();
        //    }
        //    for (int i = 0; i < size; i++)
        //    {
        //        for (int j = 0; j < size; j++)
        //        {
        //            FieldCells.Add(new FieldCellViewModel(new Extensions.Position(i, j)));
        //        }
        //    }
        //}


    }
}
