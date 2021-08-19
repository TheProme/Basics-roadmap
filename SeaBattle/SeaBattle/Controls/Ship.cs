using SeaBattle.Extensions;
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
        public IList<ShipBlockViewModel> ShipDeck
        {
            get { return (IList<ShipBlockViewModel>)GetValue(ShipDeckProperty); }
            set { SetValue(ShipDeckProperty, value); }
        }

        public static readonly DependencyProperty ShipDeckProperty =
            DependencyProperty.Register("ShipDeck", typeof(IList<ShipBlockViewModel>), typeof(Ship), new PropertyMetadata(null));



        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(Ship), new PropertyMetadata(Orientation.Horizontal));



        public Position HeadPosition
        {
            get { return (Position)GetValue(HeadPositionProperty); }
            set { SetValue(HeadPositionProperty, value); }
        }

        public static readonly DependencyProperty HeadPositionProperty =
            DependencyProperty.Register("HeadPosition", typeof(Position), typeof(Ship), new PropertyMetadata(new Position(0,0)));



        public bool Destroyed
        {
            get { return (bool)GetValue(DestroyedProperty); }
            set { SetValue(DestroyedProperty, value); }
        }

        public static readonly DependencyProperty DestroyedProperty =
            DependencyProperty.Register("Destroyed", typeof(bool), typeof(Ship), new PropertyMetadata(false));



        public ShipSize ShipSize
        {
            get { return (ShipSize)GetValue(ShipSizeProperty); }
            set { SetValue(ShipSizeProperty, value); }
        }

        public static readonly DependencyProperty ShipSizeProperty =
            DependencyProperty.Register("ShipSize", typeof(ShipSize), typeof(Ship), new PropertyMetadata(ShipSize.Tiny));



    }
}
