using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Tetris.Extensions
{
    public abstract class Field
    {
        public int Rows { get; protected set; }
        public int Columns { get; protected set; }
        protected virtual Grid BuildField(int rows, int cols)
        {
            Grid field = new Grid();
            for (int i = 0; i < rows; i++)
            {
                field.RowDefinitions.Add(new RowDefinition());
            }
            for (int j = 0; j < cols; j++)
            {
                field.ColumnDefinitions.Add(new ColumnDefinition());
            }
            return field;
        }
    }
}
