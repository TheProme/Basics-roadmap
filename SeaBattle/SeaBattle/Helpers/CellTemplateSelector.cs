using SeaBattle.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SeaBattle.Helpers
{
    public class CellTemplateSelector : DataTemplateSelector
    {
        public DataTemplate EmptyCell { get; set; }

        public DataTemplate ShipCell { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var selectedTemplate = this.EmptyCell;
            if ((item as FieldCellViewModel).CellValue != null && (item as FieldCellViewModel).CellValue is ShipBlockViewModel)
            {
                selectedTemplate = ShipCell;
            }
            return selectedTemplate;
        }
    }
}
