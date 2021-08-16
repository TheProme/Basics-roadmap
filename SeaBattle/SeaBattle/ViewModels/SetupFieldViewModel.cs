using SeaBattle.Commands;
using SeaBattle.Controls;
using SeaBattle.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace SeaBattle.ViewModels
{
    public class SetupFieldViewModel : BaseViewModel
    {
        public IEnumerable<ShipSize> ShipSizeValues
        {
            get => Enum.GetValues(typeof(ShipSize)).Cast<ShipSize>();
        }

        private ShipSize _currentShipSize;
        public ShipSize CurrentShipSize
        {
            get => _currentShipSize;
            set
            {
                _currentShipSize = value;
                SetPreviewShip(_currentShipSize);
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ShipViewModel> Ships { get; set; } = new ObservableCollection<ShipViewModel>();

        private bool _isRemoving;

        public bool IsRemoving
        {
            get => _isRemoving;
            set 
            { 
                _isRemoving = value;
                OnPropertyChanged();
            }
        }


        private int _shipsLeft;

        public int ShipsLeft
        {
            get => _shipsLeft;
            set 
            { 
                _shipsLeft = value;
                OnPropertyChanged();
            }
        }

        private Position _mousePosition;

        public Position MousePosition
        {
            get => _mousePosition;
            set 
            { 
                _mousePosition = value;
                SetShipPreview(_mousePosition);
                OnPropertyChanged();
            }
        }


        private ShipViewModel _previewShip;

        public ShipViewModel PreviewShip
        {
            get => _previewShip;
            set 
            { 
                _previewShip = value;
                OnPropertyChanged();
            }
        }


        private ShipViewModel _shipToPlace;

        public ShipViewModel ShipToPlace
        {
            get => _shipToPlace;
            set 
            { 
                _shipToPlace = value;
                OnPropertyChanged();
            }
        }

        private FieldViewModel _fieldVM;

        public FieldViewModel FieldVM
        {
            get => _fieldVM;
            set 
            { 
                _fieldVM = value;
                OnPropertyChanged();
            }
        }
        public SetupFieldViewModel(FieldViewModel fieldVM = null)
        {
            if(fieldVM == null)
            {
                fieldVM = new FieldViewModel(Ships, GameRules.FieldSize, true);
            }
            FieldVM = fieldVM;
            CurrentShipSize = ShipSize.Tiny;
            Ships.CollectionChanged += Ships_CollectionChanged;
        }

        private void Ships_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                CheckIfPossibleToPlace(CurrentShipSize);
            }
        }

        private ICommand _rotateShipCommand;

        public ICommand RotateShipCommand
        {
            get => _rotateShipCommand ?? (_rotateShipCommand = new RelayCommand(obj =>
            {
                RotateShip();
            }, obj => ShipToPlace != null || PreviewShip != null));
        }


        private void SetShipPreview(Position headDeck)
        {
            if (!IsRemoving && CheckIfPossibleToPlace(CurrentShipSize))
                ShipToPlace = new ShipViewModel(PreviewShip.Orientation, PreviewShip.ShipSize, PreviewShip.HeadPosition);
            else
            {
                ShipToPlace = null;
                FieldVM.ClearPreview();
            }
            if (ShipToPlace != null)
            {
                ShipToPlace.HeadPosition = headDeck;
                var shipDeck = ShipToPlace.ShipDeck.LastOrDefault();
                if (shipDeck != null && shipDeck.Position.Row >= FieldVM.Size)
                {
                    int rowDifference = (FieldVM.Size - 1) - shipDeck.Position.Row;
                    foreach (var deck in ShipToPlace.ShipDeck)
                    {
                        deck.Position = new Position(deck.Position.Row + rowDifference, deck.Position.Column);
                    }
                }
                if (shipDeck != null && shipDeck.Position.Column >= FieldVM.Size)
                {
                    int colDifference = (FieldVM.Size - 1) - shipDeck.Position.Column;
                    foreach (var deck in ShipToPlace.ShipDeck)
                    {
                        deck.Position = new Position(deck.Position.Row, deck.Position.Column + colDifference);
                    }
                }
                FieldVM.PreviewShip(ShipToPlace);
            }
        }

        private void SetPreviewShip(ShipSize size)
        {
            PreviewShip = new ShipViewModel(Orientation.Horizontal, size, new Position(0,0));
            CheckIfPossibleToPlace(size);
        }

        private bool CheckIfPossibleToPlace(ShipSize size)
        {
            switch (size)
            {
                case ShipSize.Tiny:
                    ShipsLeft = GameRules.TinyShips - Ships.Where(ship => ship.ShipSize == size).Count();
                    return ShipsLeft > 0;
                case ShipSize.Small:
                    ShipsLeft = GameRules.SmallShips - Ships.Where(ship => ship.ShipSize == size).Count();
                    return ShipsLeft > 0;
                case ShipSize.Medium:
                    ShipsLeft = GameRules.MediumShips - Ships.Where(ship => ship.ShipSize == size).Count();
                    return ShipsLeft > 0;
                case ShipSize.Large:
                    ShipsLeft = GameRules.LargeShips - Ships.Where(ship => ship.ShipSize == size).Count();
                    return ShipsLeft > 0;
            }
            return false;
        }

        public void PlaceShip(ShipViewModel ship)
        {
            if(FieldVM.CanPlaceShip && ship != null)
            {
                Ships.Add(ship);
                ShipToPlace = null;
            }
        }

        private void RotateShip()
        {
            if (PreviewShip.Orientation == Orientation.Horizontal)
            {
                PreviewShip.Orientation = Orientation.Vertical;
            }
            else
            {
                PreviewShip.Orientation = Orientation.Horizontal;
            }
            if(ShipToPlace != null)
                SetShipPreview(ShipToPlace.HeadPosition);
        }
    }
}
