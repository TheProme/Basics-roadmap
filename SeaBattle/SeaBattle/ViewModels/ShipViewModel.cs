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
        private const int BLOCK_SIZE = 30;
        public event Action ShipSetEvent;
        public ObservableCollection<ShipBlockViewModel> ShipBlocks { get; private set; } = new ObservableCollection<ShipBlockViewModel>();

        private Orientation _orientation;

        public Orientation Orientation
        {
            get => _orientation;
            set 
            { 
                _orientation = value;
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
                SetShipBlocksPosition(value);
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

        private bool _isSet;

        public bool IsSet
        {
            get => _isSet;
            set 
            { 
                _isSet = value;
                if (value)
                    ShipSetEvent?.Invoke();
                OnPropertyChanged();
            }
        }

        private int _deckSize = BLOCK_SIZE;

        public int DeckSize
        {
            get => _deckSize;
            set 
            { 
                _deckSize = value;
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
            if(ShipBlocks.Count > 0)
            {
                foreach (var block in ShipBlocks)
                {
                    block.BlockHitEvent -= BlockHitHandler;
                }
                ShipBlocks.Clear();
            }
            for (int i = 0; i < (int)size; i++)
            {
                ShipBlockViewModel deck = new ShipBlockViewModel(this) { BlockSize = DeckSize };
                deck.BlockHitEvent += BlockHitHandler;
                ShipBlocks.Add(deck);
            }
        }

        private void BlockHitHandler()
        {
            Destroyed = ShipBlocks.All(bl => bl.IsHit);
            //TODO: +1 ход за подбитие корабля
        }

        private void SetShipBlocksPosition(Position head)
        {
            int newRow = head.Row;
            int newCol = head.Column;
            foreach (var block in ShipBlocks)
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
        }
    }
}
