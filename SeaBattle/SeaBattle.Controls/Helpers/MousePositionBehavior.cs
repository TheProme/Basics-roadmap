using Microsoft.Xaml.Behaviors;
using SeaBattle.Controls;
using SeaBattle.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace SeaBattle.Helpers
{
    public class MousePositionBehavior : Behavior<FrameworkElement>
    {
        public Position Position
        {
            get { return (Position)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }
        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(
            "Position", typeof(Position), typeof(MousePositionBehavior), new PropertyMetadata(new Position(0, 0)));

        protected override void OnAttached()
        {
            AssociatedObject.MouseMove += AssociatedObjectOnMouseMove;
        }

        private void AssociatedObjectOnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            if(AssociatedObject is FieldGrid)
            {
                var field = AssociatedObject as FieldGrid;
                var pos = mouseEventArgs.GetPosition(field);

                var firstCell = field.Children[0];

                int x = (int)(pos.X / firstCell.RenderSize.Width);
                int y = (int)(pos.Y / firstCell.RenderSize.Height);

                Position = new Position(y, x);
            }
        }

        protected override void OnDetaching()
        {
            AssociatedObject.MouseMove -= AssociatedObjectOnMouseMove;
        }
    }
}
