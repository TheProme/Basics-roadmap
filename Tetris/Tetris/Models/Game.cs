using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using Tetris.Extensions;

namespace Tetris.Models
{
    public class Game
    {
        public bool GameIsOver { get; set; } = true;
        public int Score { get; set; }

        private GameField _gameField;

        private PreviewField _previewField;

        private Random rnd = new Random();

        public event Action LineRemoved;
        public event Action GameOverConditionReached;

        public Grid GamingGrid { get => _gameField.TetrisGrid; }
        public Grid PreviewGrid { get => _previewField.PreviewGrid; }

        private Tetrimino _nextTetrimino;
        public Tetrimino NextTetrimino 
        {
            get => _nextTetrimino;
            set
            {
                _nextTetrimino = value;
                if(value != null)
                {
                    _previewField.TetriminoPreview = ReorderTetriminoPosition(_nextTetrimino, _previewField);
                }
            }
        }

        public Game()
        {
            _gameField = new GameField();
            _previewField = new PreviewField();
        }

        private void TetriminoOverflowHandler()
        {
            GameOverConditionReached?.Invoke();
        }

        public void StopGame()
        {
            _gameField.TetriminoIsPlaced -= TetriminoIsPlacedHandler;
            _gameField.TetriminoOverflow -= TetriminoOverflowHandler;
            _gameField.LineFinished -= LineFinishedHandler;
            GamingGrid.Children.Clear();
            _gameField.PlacedBlocks.Clear();
            PreviewGrid.Children.Clear();
        }

        public void StartGame()
        {
            GameIsOver = false;
            NextTetrimino = SetNewTetrimino(0, 0);
            _gameField.TetriminoIsPlaced += TetriminoIsPlacedHandler;
            _gameField.TetriminoOverflow += TetriminoOverflowHandler;
            _gameField.LineFinished += LineFinishedHandler;
            _gameField.CurrentTetrimino = ReorderTetriminoPosition(NextTetrimino, _gameField);
        }

        private void LineFinishedHandler()
        {
            LineRemoved?.Invoke();
        }

        private void TetriminoIsPlacedHandler()
        {
            _gameField.CurrentTetrimino = ReorderTetriminoPosition(NextTetrimino, _gameField);
            if (!GameIsOver)
            {
                NextTetrimino = SetNewTetrimino(0, 0);
            }
            
        }

        private Tetrimino SetNewTetrimino(int column, int row = 0)
        {
            return new Tetrimino((TetriminoShape)rnd.Next(0,7), new Position(row, column));
        }

        private Tetrimino ReorderTetriminoPosition(Tetrimino tetrimino, Field field)
        {
            if(field is GameField)
            {
                tetrimino.Position = new Position(0, field.Columns / 2 - 1);
                return new Tetrimino(tetrimino.Base.Shape, tetrimino.Position);
            }
            if(field is PreviewField)
            {
                switch (tetrimino.Base.Shape)
                {
                    case TetriminoShape.I:
                        return new Tetrimino(tetrimino.Base.Shape, tetrimino.Position, Direction.Left);
                    case TetriminoShape.O:
                        return new Tetrimino(tetrimino.Base.Shape, new Position(0, field.Columns/2-1));
                    default:
                        return tetrimino;
                }
            }
            return tetrimino;
        }

        public void MoveTetrimino(MoveDirection direction)
        {
            if(!GameIsOver)
            {
                _gameField.MoveTetrimino(direction);
            }
        }

        public void RotateTetrimino(RotationDirection rotationDirection)
        {
            if (!GameIsOver)
            {
                _gameField.RotateTetrimino(rotationDirection);
            }
        }
    }
}
