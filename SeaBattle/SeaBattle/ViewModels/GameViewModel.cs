﻿using SeaBattle.Extensions;
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
            if (obj is EmptyCellViewModel)
            {
                SetNextTurn(player);
            }
            else
            {
                GameIsOver = player.AreShipsDestroyed();
                if (GameIsOver)
                {
                    StopGame(player);
                }
                else
                {
                    if(player is PlayerAIViewModel)
                    {
                        (player as PlayerAIViewModel).AddTurn((obj as ShipBlockViewModel));
                    }
                }
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
            SetNextTurn();
        }

        public void StopGame(PlayerViewModel player)
        {
            DisableFields();
            GameIsOver = true;

        }

        private void SetNextTurn(PlayerViewModel previousPlayer = null)
        {
            if(Players.All(player => player.PlayerTurn == false) || previousPlayer == null)
            {
                Players.First().PlayerTurn = true;
            }
            else
            {
                previousPlayer.PlayerTurn = false;
                var playerIndex = Players.IndexOf(previousPlayer);
                if (playerIndex + 1 < Players.Count())
                {
                    Players[playerIndex + 1].PlayerTurn = true;
                }
                else
                {
                    Players[0].PlayerTurn = true;
                }
            }
        }

        private void DisableFields()
        {
            foreach (var player in Players)
            {
                player.FieldPreview.FieldVM.CanClick = false;
            }
        }
    }
}
