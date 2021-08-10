using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Controls;
using Tetris.Extensions;

namespace Tetris.Models
{
    public class Block
    {
        public Rectangle Rectangle { get; private set; }
        public Color Color { get; private set; }

        private Position _position;
        public Position Position 
        {
            get => _position;
            set
            {
                _position = value;
                ChangePosition(_position);
            }
        }

        public Block(Color color, Position position)
        {
            Color = color;
            Rectangle = new Rectangle()
            {
                Fill = new SolidColorBrush(color),
                StrokeThickness = 1,
                Stroke = Brushes.White
            };
            Position = position;
        }

        public void ChangePosition(Position position)
        {
            if(position.Row >= 0 && position.Column >= 0)
            {
                Grid.SetRow(Rectangle, position.Row);
                Grid.SetColumn(Rectangle, position.Column);
            }
        }
    }
}
