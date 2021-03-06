using SeaBattle.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace SeaBattle.ViewModels
{
    public class ShipViewModel : BaseViewModel
    {
        public ObservableCollection<ShipBlockViewModel> ShipDeck { get; private set; } = new ObservableCollection<ShipBlockViewModel>();
        public ObservableCollection<Position> NeighbourCells { get; set; } = new ObservableCollection<Position>();

        public event Action<ShipViewModel> ShipDestroyedEvent;

        private Orientation _orientation;

        public Orientation Orientation
        {
            get => _orientation;
            set 
            { 
                _orientation = value;
                UpdateShip();
                OnPropertyChanged();
            }
        }

        private Position _headPosition;

        public Position HeadPosition
        {
            get => _headPosition;
            set 
            { 
                _headPosition = value;
                UpdateShip();
                OnPropertyChanged();
            }
        }

        private ShipSize _shipSize;

        public ShipSize ShipSize
        {
            get => _shipSize;
            set 
            {
                _shipSize = value;
                CreateShip(value);
                OnPropertyChanged();
            }
        }

        private bool _destroyed = false;

        public bool Destroyed
        {
            get => _destroyed;
            set 
            { 
                _destroyed = value;
                OnPropertyChanged();
            }
        }


        public ShipViewModel(Orientation orientation, ShipSize size, Position headPos)
        {
            Orientation = orientation;
            ShipSize = size;
            HeadPosition = headPos;
        }

        private void CreateShip(ShipSize size)
        {
            if(ShipDeck.Count > 0)
            {
                foreach (var block in ShipDeck)
                {
                    block.HitEvent -= BlockHitHandler;
                }
                ShipDeck.Clear();
            }
            for (int i = 0; i < (int)size; i++)
            {
                ShipBlockViewModel deck = new ShipBlockViewModel(this);
                deck.HitEvent += BlockHitHandler;
                ShipDeck.Add(deck);
            }
        }

        private void BlockHitHandler(IClickableCell clickableCell)
        {
            Destroyed = ShipDeck.All(bl => bl.IsHit);
            if(Destroyed)
            {
                ShipDestroyedEvent?.Invoke(this);
            }    
        }

        private void UpdateShip()
        {
            SetShipDeckPosition(HeadPosition);
        }

        private void SetShipDeckPosition(Position head)
        {
            NeighbourCells.Clear();
            int newRow = head.Row;
            int newCol = head.Column;
            foreach (var block in ShipDeck)
            {
                if (Orientation == Orientation.Horizontal)
                {
                    block.Position = new Position(newRow, newCol);
                    newCol++;
                }
                else
                {
                    block.Position = new Position(newRow, newCol);
                    newRow++;
                }
            }
            SetNeighbourCells();
        }

        private void SetNeighbourCells()
        {
            foreach (var deck in ShipDeck)
            {
                for (int i = deck.Position.Row - 1; i < deck.Position.Row + 2; i++)
                {
                    for (int j = deck.Position.Column - 1; j < deck.Position.Column + 2; j++)
                    {
                        Position neighbour = new Position(i, j);
                        if (GameRules.CellMatchesGameField(neighbour) &&
                            !ShipDeck.Any(block => block.Position == neighbour) && 
                            !NeighbourCells.Contains(neighbour))
                        {
                            NeighbourCells.Add(neighbour);
                        }    
                    }
                }
            }
        }
    }
}
