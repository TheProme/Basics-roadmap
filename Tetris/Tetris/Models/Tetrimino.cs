using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;
using Tetris.Extensions;

namespace Tetris.Models
{
    public class Tetrimino
    {
        public delegate void ChangedDelegate(Tetrimino tetrimino);
        public event ChangedDelegate TetriminoChanged;
        public TetriminoBase Base { get; private set; }
        public Color Color { get => Base.Color; }
        public Position Position { get; set; }
        public Direction Direction { get; set; }
        public bool IsPlaced { get; set; } = false;

        public List<Block> Figure 
        { 
            get => Base.Pattern; 
            set => Base.Pattern = value; 
        }

        public bool Move(MoveDirection moveDirection, Func<Block,bool> checkCollision)
        {
            var position = Position; //Needs to be asigned because of Base.CreateBlock
            switch (moveDirection)
            {
                case MoveDirection.Left:
                    position = new Position(position.Row, position.Column - 1);
                    break;
                case MoveDirection.Right:
                    position = new Position(position.Row, position.Column + 1);
                    break;
                case MoveDirection.Down:
                    position = new Position(position.Row + 1, position.Column);
                    break;
            }
            var newPattern = Base.CreateBlock(Base.Shape, position, Direction);
            if (newPattern.Any(checkCollision))
            {
                TetriminoChanged?.Invoke(this); //Redraw base tetrimino on same position
                return false;
            }


            Position = position;
            Figure = newPattern;
            TetriminoChanged?.Invoke(this); //Redraw new tetrimino
            return true;
        }

        public bool SwitchDirection(RotationDirection rotationDirection, Func<Block, bool> checkCollision)
        {
            var count = Enum.GetValues(typeof(Direction)).Length;
            var delta = (rotationDirection == RotationDirection.Right) ? 1 : -1;
            var direction = (int)this.Direction + delta;
            if (direction < 0) direction += count;
            if (direction >= count) direction %= count;

            //Adjust tetrimino position if it collides with walls or whatever. Move block away for 0, 1, -1... rows
            var adjustPattern = Base.Shape == TetriminoShape.I ? new[] { 0, 1, -1, 2, -2 } : new[] { 0, 1, -1 };
            foreach (var adjust in adjustPattern)
            {
                var position = new Position(this.Position.Row, this.Position.Column + adjust);
                var blocks = Base.CreateBlock(Base.Shape, position, (Direction)direction);

                if (!blocks.Any(checkCollision))
                {
                    Direction = (Direction)direction;
                    Position = position;
                    Figure = blocks;
                    TetriminoChanged?.Invoke(this);
                    return true;
                }
            }
            TetriminoChanged?.Invoke(this);
            return false;
        }

        public Tetrimino(TetriminoShape shape, Position position, Direction direction = Direction.Up)
        {
            Position = position;
            Direction = direction;
            Base = new TetriminoBase(shape, Position, Direction);
        }
    }
}
