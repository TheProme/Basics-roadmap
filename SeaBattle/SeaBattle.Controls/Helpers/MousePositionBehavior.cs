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
            if(AssociatedObject is Field)
            {
                var field = AssociatedObject as Field;
                var pos = mouseEventArgs.GetPosition(field);

                var firstCell = field.FieldCells.FirstOrDefault();

                int row = 0;
                int col = 0;
                double accumulatedHeight = 0.0;
                double accumulatedWidth = 0.0;

                int x = (int)(pos.X / firstCell.CellSize.Width);
                int y = (int)(pos.Y / firstCell.CellSize.Height);

                foreach (var cell in field.FieldCells)
                {
                    accumulatedHeight += cell.CellSize.Height;
                    if (accumulatedHeight > pos.Y)
                        break;
                    row++;
                }

                foreach (var columnDefinition in field.FieldCells)
                {
                    accumulatedWidth += columnDefinition.CellSize.Width;
                    if (accumulatedWidth > pos.X)
                        break;
                    col++;
                }
                Position = new Position(y, x);
            }
        }

        protected override void OnDetaching()
        {
            AssociatedObject.MouseMove -= AssociatedObjectOnMouseMove;
        }
    }
}
