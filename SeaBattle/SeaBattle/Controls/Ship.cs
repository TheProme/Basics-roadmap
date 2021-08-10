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
    public class Ship : Control
    {
        static Ship()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Ship), new FrameworkPropertyMetadata(typeof(Ship)));
        }

        private List<ShipBlock> ShipBlocks = new List<ShipBlock>();

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _shipBase = GetTemplateChild(PART_ShipBase) as StackPanel;
            CreateShipBlocks((int)Size, _shipBase);
        }

        protected const string PART_ShipBase = nameof(PART_ShipBase);
        private StackPanel _shipBase = null;

        public ShipSize Size
        {
            get { return (ShipSize)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register("NumberOfBlocks", typeof(ShipSize), typeof(Ship), new PropertyMetadata(ShipSize.Tiny, SetShipBlocks));

        private static void SetShipBlocks(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Ship).SetValue(SizeProperty, (ShipSize)e.NewValue);
        }

        public bool Destroyed
        {
            get { return (bool)GetValue(DestroyedProperty); }
            set { SetValue(DestroyedProperty, value); }
        }

        public static readonly DependencyProperty DestroyedProperty =
            DependencyProperty.Register("Destroyed", typeof(bool), typeof(Ship), new PropertyMetadata(false, DestroyedChanged));

        private static void DestroyedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (d as Ship);
            control.SetValue(DestroyedProperty, e.NewValue);
            if((bool)e.NewValue)
            {
                foreach (var item in control.ShipBlocks)
                {
                    item.BlockHitEvent -= control.BlockHitHandler;
                }
            }
        }

        public Orientation ShipOrientation
        {
            get { return (Orientation)GetValue(ShipOrientationProperty); }
            set { SetValue(ShipOrientationProperty, value); }
        }

        public static readonly DependencyProperty ShipOrientationProperty =
            DependencyProperty.Register("ShipOrientation", typeof(Orientation), typeof(Ship), new PropertyMetadata(Orientation.Vertical));

        public int BlockSize
        {
            get { return (int)GetValue(BlockSizeProperty); }
            set { SetValue(BlockSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BlockSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BlockSizeProperty =
            DependencyProperty.Register("BlockSize", typeof(int), typeof(Ship), new PropertyMetadata(10));



        private void CreateShipBlocks(int numberOfBlocks, StackPanel shipBase)
        {
            for (int i = 0; i < numberOfBlocks; i++)
            {
                ShipBlock block = new ShipBlock() { Width = BlockSize, Height = BlockSize };
                block.BlockHitEvent += BlockHitHandler;
                ShipBlocks.Add(block);
                shipBase.Children.Add(block);
            }
        }

        private void BlockHitHandler()
        {
            Destroyed = ShipBlocks.All(bl => bl.IsHit);
            //TODO: +1 ход за подбитие корабля
        }
    }
}
