using SeaBattle.Extensions;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace SeaBattle.Controls
{
    public class FieldGrid : UniformGrid
    {
        static FieldGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FieldGrid), new FrameworkPropertyMetadata(typeof(FieldGrid)));
        }

        protected override Size MeasureOverride(Size constraint)
        {
            var minorValue = constraint.Height < constraint.Width ? constraint.Height : constraint.Width;
            constraint = new Size(minorValue, minorValue);
            return base.MeasureOverride(constraint);
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            var minorValue = arrangeSize.Height < arrangeSize.Width ? arrangeSize.Height : arrangeSize.Width;
            arrangeSize = new Size(minorValue, minorValue);
            foreach (UIElement cell in InternalChildren)
            {
                cell.Arrange(new Rect(new Size((int)arrangeSize.Width / GameRules.FieldCellsCount, (int)arrangeSize.Height / GameRules.FieldCellsCount)));
            }
            return base.ArrangeOverride(arrangeSize);
        }
    }
}
