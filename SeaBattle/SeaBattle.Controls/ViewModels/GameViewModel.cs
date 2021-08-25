using SeaBattle.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace SeaBattle.ViewModels
{
    public class GameViewModel : BaseViewModel
    {
        public ObservableCollection<PlayerViewModel> Players { get; set; } = new ObservableCollection<PlayerViewModel>();

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


        private bool _gameIsOver;

        public bool GameIsOver
        {
            get => _gameIsOver;
            set
            {
                _gameIsOver = value;
                OnPropertyChanged();
            }
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
            Players.Add(new PlayerViewModel(playerField, aiField));
            Players.Add(new PlayerAIViewModel(aiField, playerField));
            
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
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
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
                StopGame(Players.FirstOrDefault(p => p.AreShipsDestroyed()));
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
            SetNextTurn(Players.LastOrDefault());
        }

        public void StopGame(PlayerViewModel player)
        {
            GameIsOver = true;
            StopTurns();

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
                    player.OpponentField.CanClick = true;
                }
            }
        }

        private void DisableFields(PlayerViewModel player)
        {
            player.FieldPreview.CanClick = false;
            player.OpponentField.CanClick = false;
        }

        private void StopTurns()
        {
            foreach (var player in Players)
            {
                player.PlayerTurn = false;
                DisableFields(player);
            }
        }
    }
}
