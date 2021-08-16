using SeaBattle.Commands;
using SeaBattle.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace SeaBattle.ViewModels
{
    public class FieldViewModel : BaseViewModel
    {
        private bool _isPlayerField;

        public bool IsPlayerField
        {
            get => _isPlayerField;
            set 
            { 
                _isPlayerField = value;
                OnPropertyChanged();
            }
        }

        private bool _isReady;

        public bool IsReady
        {
            get => _isReady;
            set
            {
                _isReady = value;
                OnPropertyChanged();
            }
        }

        private bool _canPlaceShip;

        public bool CanPlaceShip
        {
            get => _canPlaceShip; 
            set 
            { 
                _canPlaceShip = value;
                OnPropertyChanged();
            }
        }


        private int _size = 10;

        public int Size
        {
            get => _size;
            set 
            { 
                _size = value;
                CreateField(value);
                OnPropertyChanged();
            }
        }

        public ObservableCollection<FieldCellViewModel> FieldCells { get; set; } = new ObservableCollection<FieldCellViewModel>();

        public ObservableCollection<ShipViewModel> Ships { get; set; } = new ObservableCollection<ShipViewModel>();


        public FieldViewModel(ObservableCollection<ShipViewModel> ships, int size, bool isPlayerField = false)
        {
            Ships = ships;
            Ships.CollectionChanged += ShipsChanged;
            Size = size;
            IsPlayerField = isPlayerField;
        }

        private void SetShipsOnField(ObservableCollection<ShipViewModel> ships)
        {
            foreach (var ship in ships)
            {
                AddShipToField(ship);
            }
        }

        private void ShipsChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    foreach (var item in e.NewItems)
                    {
                        if(item != null)
                        {
                            AddShipToField(item as ShipViewModel);
                        }
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        if(item != null)
                        {
                            RemoveShipFromField(item as ShipViewModel);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private void CreateField(int size)
        {
            FieldCells.Clear();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    var emptyCell = new EmptyCellViewModel(new Extensions.Position(i, j));
                    var cell = new FieldCellViewModel(emptyCell.Position, emptyCell);
                    FieldCells.Add(cell);
                }
            }
            SetShipsOnField(Ships);
        }

        private void AddShipToField(ShipViewModel ship)
        {
            foreach (var deck in ship.ShipDeck)
            {
                var existingCell = FieldCells.FirstOrDefault(cell => cell.Position == deck.Position);
                if(existingCell != null)
                {
                    var index = FieldCells.IndexOf(existingCell);
                    FieldCells.Remove(existingCell);
                    var shipCell = new FieldCellViewModel(deck.Position, deck);
                    FieldCells.Insert(index, shipCell);
                }
            }
        }

        private void RemoveShipFromField(ShipViewModel ship)
        {
            foreach (var deck in ship.ShipDeck)
            {
                var existingCell = FieldCells.FirstOrDefault(cell => cell.Position == deck.Position);
                if (existingCell != null)
                {
                    var index = FieldCells.IndexOf(existingCell);
                    var emptyCell = new EmptyCellViewModel(deck.Position);
                    FieldCells.Remove(existingCell);
                    var cell = new FieldCellViewModel(emptyCell.Position, emptyCell);
                    FieldCells.Insert(index, cell);
                }
            }
        }

        public void ClearPreview()
        {
            foreach (var cell in FieldCells)
            {
                cell.IsPreview = false;
            }
        }

        public void PreviewShip(ShipViewModel ship)
        {
            ClearPreview();
            foreach (var deck in ship.ShipDeck)
            {
                var existingCell = FieldCells.FirstOrDefault(cell => cell.Position == deck.Position);
                if(existingCell != null)
                {
                    foreach (var existingShip in Ships)
                    {
                        if (existingShip.ShipDeck.Any(d => d.Position == existingCell.Position) || existingShip.NeighbourCells.Any(c => c == existingCell.Position))
                        {
                            existingCell.IsOccupied = true;
                            break;
                        }
                    }
                    existingCell.IsPreview = true;
                    CanPlaceShip = !existingCell.IsOccupied;
                }
            }
        }

        public ShipViewModel GetShipByPosition(Position position)
        {
            return Ships.FirstOrDefault(ship => ship.ShipDeck.Any(deck => deck.Position == position));
        }
    }
}
