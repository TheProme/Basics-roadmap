using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using Tetris.Extensions;

namespace Tetris.Models
{
    public class GameField : Field
    {
        public readonly int RowsCount = 24;
        public readonly int ColumnsCount = 10;

        public List<Block> PlacedBlocks = new List<Block>();

        public event Action TetriminoIsPlaced;
        public event Action TetriminoOverflow;
        public event Action LineFinished;

        private Tetrimino _currentTetrimino;
        public Tetrimino CurrentTetrimino 
        {
            get => _currentTetrimino;
            set
            {
                if (_currentTetrimino != null)
                {
                    _currentTetrimino.TetriminoChanged -= TetrominoChangedHandler;
                }
                _currentTetrimino = value;
                if (_currentTetrimino.Figure.Any(ReachedBottomOrBlock))
                {
                    TetriminoOverflow?.Invoke();
                }
                else
                {
                    _currentTetrimino.TetriminoChanged += TetrominoChangedHandler;
                    AddTetrimino(_currentTetrimino);
                }
                
            }
        }

        private void TetrominoChangedHandler(Tetrimino tetrimino)
        {
            foreach (var item in tetrimino.Figure)
            {
                TetrisGrid.Children.Add(item.Rectangle);
            }
        }

        public void AddTetrimino(Tetrimino tetrimino)
        {
            foreach (var item in tetrimino.Figure)
            {
                TetrisGrid.Children.Add(item.Rectangle);
            }
        }

        public Grid TetrisGrid { get; private set; }

        public GameField(int rows = 0, int columns = 0)
        {
            RowsCount = rows > columns && rows > 6 ? rows : RowsCount;
            Rows = RowsCount;
            ColumnsCount = columns > 4 ? columns : ColumnsCount;
            Columns = ColumnsCount;
            TetrisGrid = BuildField(Rows, Columns);
        }
        public bool MoveTetrimino(MoveDirection direction)
        {
            if (CurrentTetrimino != null)
            {
                foreach (var item in CurrentTetrimino.Figure)
                {
                    TetrisGrid.Children.Remove(item.Rectangle);
                }
                if (direction != MoveDirection.Down)
                {
                    return CurrentTetrimino.Move(direction, CheckCollision);
                    
                }
                if (!CurrentTetrimino.Move(direction, ReachedBottomOrBlock))
                {
                    SetTetrimino(CurrentTetrimino);
                    TetriminoIsPlaced?.Invoke();
                    return true;
                }
            }
            return false;
        }

        public void RotateTetrimino(RotationDirection rotationDirection)
        {
            foreach (var item in CurrentTetrimino.Figure)
            {
                TetrisGrid.Children.Remove(item.Rectangle);
            }
            CurrentTetrimino.SwitchDirection(rotationDirection, CheckCollision);
        }

        private bool CheckCollision(Block block)
        {
            if (block.Position.Column >= ColumnsCount || block.Position.Column < 0)
            {
                return true;
            }
            if (PlacedBlocks.Any(b => b.Position == block.Position))
            {
                return true;
            }
            return false;
        }

        public bool ReachedBottomOrBlock(Block block)
        {
            if (block.Position.Row == RowsCount || PlacedBlocks.Any(b => CurrentTetrimino.Figure.Any(block => block.Position.Row + 1 == b.Position.Row && block.Position.Column == b.Position.Column)))
            {
                return true;
            }
            return false;
        }

        private void SetTetrimino(Tetrimino tetrimino)
        {
            foreach (var figureBlock in tetrimino.Figure)
            {
                figureBlock.Position = new Position(figureBlock.Position.Row, figureBlock.Position.Column);
                PlacedBlocks.Add(figureBlock);
            }
            tetrimino.IsPlaced = true;
            int previousRow = 0;
            foreach (var block in tetrimino.Figure)
            {
                if (previousRow != block.Position.Row)
                {
                    previousRow = block.Position.Row;
                    RemoveLine(previousRow);
                }
            }
        }
        private void RemoveLine(int row)
        {
            var bottomLine = PlacedBlocks.Where(b => b.Position.Row == row).ToList();
            if (bottomLine.Count() == ColumnsCount)
            {
                foreach (var item in PlacedBlocks.Where(block => block.Position.Row <= row))
                {
                    TetrisGrid.Children.Remove(item.Rectangle);
                }
                foreach (var item in bottomLine)
                {
                    PlacedBlocks.Remove(item);
                }
                foreach (var block in PlacedBlocks)
                {
                    if(block.Position.Row <= row)
                    {
                        block.Position = new Position(block.Position.Row + 1, block.Position.Column);
                        TetrisGrid.Children.Add(block.Rectangle);
                    }
                }
                LineFinished?.Invoke();
                RemoveLine(row);
            }
        }
    }
}
