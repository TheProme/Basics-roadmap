using SeaBattle.Commands;
using SeaBattle.Controls;
using SeaBattle.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace SeaBattle.ViewModels
{
    public class GameViewModel : BaseViewModel
    {
        public ObservableCollection<PlayerViewModel> Players { get; set; } = new ObservableCollection<PlayerViewModel>();

        private PlayerViewModel _player;

        public PlayerViewModel Player
        {
            get => _player;
            set
            {
                _player = value;
                OnPropertyChanged();
            }
        }

        private PlayerAIViewModel _ai;

        public PlayerAIViewModel AI
        {
            get => _ai;
            set 
            { 
                _ai = value;
                OnPropertyChanged();
            }
        }


        private PlayerViewModel _activePlayer;

        public PlayerViewModel ActivePlayer
        {
            get => _activePlayer;
            set 
            { 
                _activePlayer = value;
                OnPropertyChanged();
            }
        }

        private bool _gameStarted = false;

        public bool GameStarted
        {
            get => _gameStarted;
            set 
            { 
                _gameStarted = value;
                OnPropertyChanged();
            }
        }

        private bool _currentlyPlaying = false;

        public bool CurrentlyPlaying
        {
            get => _currentlyPlaying; 
            set 
            {
                _currentlyPlaying = value;
                OnPropertyChanged();
            }
        }


        private bool _gameIsOver = false;

        public bool GameIsOver
        {
            get => _gameIsOver;
            set
            {
                _gameIsOver = value;
                OnPropertyChanged();
            }
        }

        private ICommand _startGameCommand;

        public ICommand StartGameCommand
        {
            get => _startGameCommand ?? (_startGameCommand = new RelayCommand(obj =>
            {
                GameStarted = true;
            }));
        }

        private ICommand _exitCommand;

        public ICommand ExitCommand
        {
            get => _exitCommand ?? (_exitCommand = new RelayCommand(obj =>
            {
                System.Windows.Application.Current.Shutdown();
            }));
        }

        private ICommand _restartCommand;

        public ICommand RestartCommand
        {
            get => _restartCommand ?? (_restartCommand = new RelayCommand(obj =>
            {
                RestartGame();
            }));
        }

        private void RestartGame()
        {
            ActivePlayer = null;
            CurrentlyPlaying = false;
            GameIsOver = false;
            AI.ClearField();
            Player.ClearField();
            Player.FieldPreview.CanClick = true;
            Player.OpponentField.CanClick = false;
            //foreach (var player in Players)
            //{
            //    UnsetPlayerEvents(player);
            //}
            //Players.Clear();
            //AddPlayers();
        }

        public GameViewModel()
        {
            Players.CollectionChanged += PlayersCollectionUpdated;
            AddPlayers();
        }

        private void AddPlayers()
        {
            SetupFieldViewModel playerField = new SetupFieldViewModel(true);
            SetupFieldViewModel aiField = new SetupFieldViewModel(false);
            Player = new PlayerViewModel(playerField, aiField);
            AI = new PlayerAIViewModel(aiField, playerField);
            Players.Add(Player);
            Players.Add(AI);
        }

        private void SetPlayerEvents(PlayerViewModel player)
        {
            player.FireEvent += PlayerFireHandler;
            if(!(player is PlayerAIViewModel))
            {
                player.PlayerReadyEvent += PlayerIsReady;
            }
        }

        private void UnsetPlayerEvents(PlayerViewModel player)
        {
            player.FireEvent -= PlayerFireHandler;
            player.PlayerReadyEvent -= PlayerIsReady;
        }

        private void PlayersCollectionUpdated(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                    {
                        SetPlayerEvents(item as PlayerViewModel);
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        UnsetPlayerEvents(item as PlayerViewModel);
                    }
                    break;
            }
        }

        private void PlayerFireHandler(IClickableCell obj, PlayerViewModel player)
        {
            if (Players.Any(p => p.AreShipsDestroyed()))
            {
                StopGame();
                return;
            }

            if (obj is EmptyCellViewModel)
            {
                SetNextTurn(player);
            }
            else if (player is PlayerAIViewModel)
            {
                (player as PlayerAIViewModel).AddTurn(obj as ShipBlockViewModel);
            }
        }

        private void PlayerIsReady(PlayerViewModel obj)
        {
            if(ArePlayersReady(obj))
            {
                StartGame();
            }
        }

        private bool ArePlayersReady(PlayerViewModel player)
        {
            if (Players.Where(p => !(p is PlayerAIViewModel)).All(player => player.FieldIsSet))
            {
                foreach (var ai in Players.Where(p => p is PlayerAIViewModel))
                {
                    (ai as PlayerAIViewModel).SetAIReady();
                }
                return true;
            }
            return false;
        }



        public void StartGame()
        {
            CurrentlyPlaying = true;
            SetNextTurn(Players.LastOrDefault());
        }

        public void StopGame()
        {
            CurrentlyPlaying = false;
            GameIsOver = true;
            StopTurns();
            foreach (var p in Players)
            {
                DisableFields(p);
            }
        }

        private void SetNextTurn(PlayerViewModel previousPlayer)
        {
            if(!GameIsOver)
            {
                PlayerViewModel nextPlayer = null;
                if (Players.All(player => player.PlayerTurn == false) || previousPlayer == null)
                {
                    nextPlayer = Players.First();
                }
                else
                {
                    previousPlayer.PlayerTurn = false;
                    var playerIndex = Players.IndexOf(previousPlayer);
                    if (playerIndex + 1 < Players.Count())
                    {
                        nextPlayer = Players[playerIndex + 1];
                    }
                    else
                    {
                        nextPlayer = Players[0];
                    }
                }

                nextPlayer.PlayerTurn = true;
                ActivateFieldsBasedOnTurn();
                ActivePlayer = nextPlayer;
            }
        }

        private void ActivateFieldsBasedOnTurn()
        {
            foreach (var player in Players)
            {
                if(player.PlayerTurn)
                {

                    player.FieldPreview.CanClick = false;
                    //Blocks clicking on player field while AI's turn
                    if(player is PlayerAIViewModel)
                    {
                        player.OpponentField.CanClick = false;
                    }
                    else
                    {
                        player.OpponentField.CanClick = true;
                    }
                }
            }
        }

        private void DisableFields(PlayerViewModel player)
        {
            player.FieldPreview.FieldVM.ClearFog = true;
            player.FieldPreview.CanClick = false;
            player.OpponentField.CanClick = false;
        }

        private void StopTurns()
        {
            foreach (var player in Players)
            {
                player.PlayerTurn = false;
            }
        }
    }
}
