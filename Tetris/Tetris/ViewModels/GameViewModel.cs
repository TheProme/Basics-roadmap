using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Tetris.Extensions;

namespace Tetris.Models
{
    public class GameViewModel : INotifyPropertyChanged
    {
        public Game Game { get; private set; }
        public GameTimer Timer { get; private set; }
        public int Score
        {
            get => Game.Score;
            set
            {
                Game.Score = value;
                OnPropertyChanged();
            }
        }
        public bool GameIsOver
        {
            get => Game.GameIsOver;
            set 
            {
                Game.GameIsOver = value;
                OnPropertyChanged();
            }
        }

        private bool _showGameOver = false;
        public bool ShowGameOver 
        {
            get => _showGameOver;
            set
            {
                _showGameOver = value;
                OnPropertyChanged();
            }
        }

        public void Start()
        {
            ShowGameOver = false;
            if(!GameIsOver)
            {
                Stop();
            }
            GameIsOver = false;
            Game.GameOverConditionReached += GameOverHandler;
            Game.LineRemoved += LineRemovedHandler;
            Timer.MoveTickEvent += MoveTickHandler;
            Score = 0;
            Game.StartGame();
            Timer.StartTimer();

        }

        public void Stop()
        {
            GameIsOver = true;
            Game.GameOverConditionReached -= GameOverHandler;
            Game.LineRemoved -= LineRemovedHandler;
            Timer.MoveTickEvent -= MoveTickHandler;
            Game.StopGame();
            Timer.StopTimer();
        }


        public GameViewModel(Game game, GameTimer timer)
        {
            Game = game;
            Timer = timer;
        }

        private void LineRemovedHandler()
        {
            Score += 100;
        }

        private void MoveTickHandler()
        {
            App.Current.Dispatcher.Invoke(()=> { Game.MoveTetrimino(MoveDirection.Down); });
        }

        private void GameOverHandler()
        {
            Stop();
            ShowGameOver = true;
        }

        public void Exit()
        {
            Stop();
            App.Current.Shutdown();
        }

        private ICommand _keyPressed;
        public ICommand KeyPressed
        {
            get => _keyPressed ?? (_keyPressed = new ParametrizedCommand(param => { KeyInput((KeyEventArgs)param); }, (param) => param is KeyEventArgs));
        }

        private ICommand _startGameCommand;
        public ICommand StartGameCommand
        {
            get => _startGameCommand ?? (_startGameCommand = new ParametrizedCommand((obj) => { Start(); }));
        }

        private ICommand _stopGameCommand;
        public ICommand StopGameCommand
        {
            get => _stopGameCommand ?? (_stopGameCommand = new ParametrizedCommand((obj) => { Stop(); }));
        }

        private ICommand _exitCommand;
        public ICommand ExitCommand
        {
            get => _exitCommand ?? (_exitCommand = new ParametrizedCommand((obj) => { Exit(); }));
        }

        public void KeyInput(KeyEventArgs keyArgs)
        {
            switch (keyArgs.Key)
            {
                case Key.Up:
                    Game.RotateTetrimino(RotationDirection.Right);
                    break;
                case Key.Down:
                    Game.MoveTetrimino(MoveDirection.Down);
                    break;
                case Key.Left:
                    Game.MoveTetrimino(MoveDirection.Left);
                    break;
                case Key.Right:
                    Game.MoveTetrimino(MoveDirection.Right);
                    break;
                default:
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
