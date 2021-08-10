using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using Tetris.Models;

namespace Tetris.Extensions
{
    public enum Direction
    {
        Left,
        Up,
        Right,
        Down
    }
    public enum RotationDirection
    {
        Left,
        Right
    }
    public enum MoveDirection
    {
        Left,
        Right,
        Down
    }
    public enum TetriminoShape
    {
        I,
        O,
        S,
        Z,
        J,
        L,
        T
    }

    public class TetriminoBase
    {
        public TetriminoShape Shape { get; private set; }
        public Color Color { get; private set; }
        public List<Block> Pattern { get; set; }

        public TetriminoBase(TetriminoShape shape, Position basePosition, Direction direction = Direction.Up)
        {
            SetTetriminoBase(shape, basePosition, direction);
        }

        private void SetTetriminoBase(TetriminoShape shape, Position basePosition, Direction direction)
        {
            Shape = shape;
            Color = ReturnBlockColor(shape);
            Pattern = CreateBlock(shape, basePosition, direction);
        }
        public List<Block> CreateBlock(TetriminoShape shape, Position startPosition, Direction direction)
        {
            int[,] pattern = null;
            switch (shape)
            {
                case TetriminoShape.I:
                    switch (direction)
                    {
                        case Direction.Up:
                            pattern = new int[,]
                            {
                                { 0, 1, 0, 0 },
                                { 0, 1, 0, 0 },
                                { 0, 1, 0, 0 },
                                { 0, 1, 0, 0 },
                            };
                            break;
                        case Direction.Right:
                            pattern = new int[,]
                            {
                                { 0, 0, 0, 0 },
                                { 1, 1, 1, 1 },
                                { 0, 0, 0, 0 },
                                { 0, 0, 0, 0 },
                            };
                            break;
                        case Direction.Down:
                            pattern = new int[,]
                            {
                                { 0, 0, 1, 0 },
                                { 0, 0, 1, 0 },
                                { 0, 0, 1, 0 },
                                { 0, 0, 1, 0 },
                            };
                            break;
                        case Direction.Left:
                            pattern = new int[,]
                            {
                                { 0, 0, 0, 0 },
                                { 0, 0, 0, 0 },
                                { 1, 1, 1, 1 },
                                { 0, 0, 0, 0 },
                            };
                            break;
                    }
                    break;
                case TetriminoShape.O:
                    pattern = new int[,] 
                    { 
                        { 1, 1 }, 
                        { 1, 1 } 
                    };
                    break;
                case TetriminoShape.S:
                    switch (direction)
                    {
                        case Direction.Up:
                            pattern = new int[,]
                            {
                                { 0, 1, 1 },
                                { 1, 1, 0 },
                                { 0, 0, 0 },
                            };
                            break;

                        case Direction.Right:
                            pattern = new int[,]
                            {
                                { 0, 1, 0 },
                                { 0, 1, 1 },
                                { 0, 0, 1 },
                            };
                            break;

                        case Direction.Down:
                            pattern = new int[,]
                            {
                                { 0, 0, 0 },
                                { 0, 1, 1 },
                                { 1, 1, 0 },
                            };
                            break;

                        case Direction.Left:
                            pattern = new int[,]
                            {
                                { 1, 0, 0 },
                                { 1, 1, 0 },
                                { 0, 1, 0 },
                            };
                            break;
                    }
                    break;
                case TetriminoShape.Z:
                    switch (direction)
                    {
                        case Direction.Up:
                            pattern = new int[,]
                            {
                                { 1, 1, 0 },
                                { 0, 1, 1 },
                                { 0, 0, 0 },
                            };
                            break;

                        case Direction.Right:
                            pattern = new int[,]
                            {
                                { 0, 0, 1 },
                                { 0, 1, 1 },
                                { 0, 1, 0 },
                            };
                            break;

                        case Direction.Down:
                            pattern = new int[,]
                            {
                                { 0, 0, 0 },
                                { 1, 1, 0 },
                                { 0, 1, 1 },
                            };
                            break;

                        case Direction.Left:
                            pattern = new int[,]
                            {
                                { 0, 1, 0 },
                                { 1, 1, 0 },
                                { 1, 0, 0 },
                            };
                            break;
                    }
                    break;
                case TetriminoShape.J:
                    switch (direction)
                    {
                        case Direction.Up:
                            pattern = new int[,]
                            {
                                { 1, 0, 0 },
                                { 1, 1, 1 },
                                { 0, 0, 0 },
                            };
                            break;

                        case Direction.Right:
                            pattern = new int[,]
                            {
                                { 0, 1, 1 },
                                { 0, 1, 0 },
                                { 0, 1, 0 },
                            };
                            break;

                        case Direction.Down:
                            pattern = new int[,]
                            {
                                { 0, 0, 0 },
                                { 1, 1, 1 },
                                { 0, 0, 1 },
                            };
                            break;

                        case Direction.Left:
                            pattern = new int[,]
                            {
                                { 0, 1, 0 },
                                { 0, 1, 0 },
                                { 1, 1, 0 },
                            };
                            break;
                    }
                    break;
                case TetriminoShape.L:
                    switch (direction)
                    {
                        case Direction.Up:
                            pattern = new int[,]
                            {
                                { 0, 0, 1 },
                                { 1, 1, 1 },
                                { 0, 0, 0 },
                            };
                            break;

                        case Direction.Right:
                            pattern = new int[,]
                            {
                                { 0, 1, 0 },
                                { 0, 1, 0 },
                                { 0, 1, 1 },
                            };
                            break;

                        case Direction.Down:
                            pattern = new int[,]
                            {
                                { 0, 0, 0 },
                                { 1, 1, 1 },
                                { 1, 0, 0 },
                            };
                            break;

                        case Direction.Left:
                            pattern = new int[,]
                            {
                                { 1, 1, 0 },
                                { 0, 1, 0 },
                                { 0, 1, 0 },
                            };
                            break;
                    }
                    break;
                case TetriminoShape.T:
                    switch (direction)
                    {
                        case Direction.Up:
                            pattern = new int[,]
                            {
                                { 0, 1, 0 },
                                { 1, 1, 1 },
                                { 0, 0, 0 },
                            };
                            break;

                        case Direction.Right:
                            pattern = new int[,]
                            {
                                { 0, 1, 0 },
                                { 0, 1, 1 },
                                { 0, 1, 0 },
                            };
                            break;

                        case Direction.Down:
                            pattern = new int[,]
                            {
                                { 0, 0, 0 },
                                { 1, 1, 1 },
                                { 0, 1, 0 },
                            };
                            break;

                        case Direction.Left:
                            pattern = new int[,]
                            {
                                { 0, 1, 0 },
                                { 1, 1, 0 },
                                { 0, 1, 0 },
                            };
                            break;
                    }
                    break;
                default:
                    throw new InvalidOperationException("Unknown tetrimino shape");
            }
            return Enumerable.Range(0, pattern.GetLength(0))
                    .SelectMany(rows => Enumerable.Range(0, pattern.GetLength(1)).Select(cols => new Position(rows, cols)))
                    .Where(pos => pattern[pos.Row, pos.Column] != 0)
                    .Select(newPos => new Position(newPos.Row + startPosition.Row, newPos.Column + startPosition.Column))
                    .Select(newPos => new Block(Color, newPos))
                    .ToList();
        }

        private Color ReturnBlockColor(TetriminoShape shape)
        {
            switch (shape)
            {
                case TetriminoShape.I: 
                    return (Color)ColorConverter.ConvertFromString("#d19e11");
                case TetriminoShape.O: 
                    return (Color)ColorConverter.ConvertFromString("#b32222");
                case TetriminoShape.S: 
                    return (Color)ColorConverter.ConvertFromString("#0f8511");
                case TetriminoShape.Z: 
                    return (Color)ColorConverter.ConvertFromString("#1e4e9c");
                case TetriminoShape.J: 
                    return (Color)ColorConverter.ConvertFromString("#3ebdc2");
                case TetriminoShape.L: 
                    return (Color)ColorConverter.ConvertFromString("#a8ba02");
                case TetriminoShape.T: 
                    return (Color)ColorConverter.ConvertFromString("#989993");
                default: 
                    throw new InvalidOperationException("Unknown tetrimino shape");
            }
        }
    }
}
