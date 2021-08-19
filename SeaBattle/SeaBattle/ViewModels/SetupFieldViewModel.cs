using SeaBattle.Commands;
using SeaBattle.Controls;
using SeaBattle.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

        public event Action FieldIsReadyEvent;

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

        private Random rnd = new Random();

        public SetupFieldViewModel(bool isPlayerField)
        {
            if(isPlayerField)
            {
                FieldVM = new FieldViewModel(Ships, GameRules.FieldSize, true);
                CurrentShipSize = ShipSize.Tiny;
            }
            else
            {
                FieldVM = new FieldViewModel(Ships, GameRules.FieldSize, false);
                RandomizePlacement();
            }
            
        }

        private void RandomizePlacement()
        {
            FieldVM.ClearField();
            foreach (var size in ShipSizeValues.Reverse())
            {
                switch (size)
                {
                    case ShipSize.Tiny:
                        PlaceRandomShips(size, GameRules.TinyShips);
                        break;
                    case ShipSize.Small:
                        PlaceRandomShips(size, GameRules.SmallShips);
                        break;
                    case ShipSize.Medium:
                        PlaceRandomShips(size, GameRules.MediumShips);
                        break;
                    case ShipSize.Large:
                        PlaceRandomShips(size, GameRules.LargeShips);
                        break;
                }
            }
            CheckIfPossibleToPlace(CurrentShipSize);
        }
        private void PlaceRandomShips(ShipSize shipSize, int shipsCount)
        {
            int tries = 0;
            CurrentShipSize = shipSize;
            for (int i = 0; i < shipsCount; i++)
            {
                bool canPlace = true;
                PreviewShip = new ShipViewModel((Orientation)rnd.Next(0, 2), shipSize, new Position(0,0));
                do
                {
                    PreviewShip.HeadPosition = new Position(rnd.Next(GameRules.FieldSize - 1), rnd.Next(GameRules.FieldSize - 1));
                    canPlace = SetShipPreview(PreviewShip.HeadPosition);
                    if (!canPlace)
                    {
                        RotateShip();
                    }
                    tries++;
                    if(tries > 300)
                    {
                        break;
                    }
                }
                while (!canPlace);
                if(tries > 300)
                {
                    RandomizePlacement();
                }
                else
                {
                    PlaceShip(ShipToPlace);
                }
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

        private ICommand _randomizePlacementCommand;

        public ICommand RandomizePlacementCommand
        {
            get => _randomizePlacementCommand ?? (_randomizePlacementCommand = new RelayCommand(obj =>
            {
                RandomizePlacement();
            }, obj => !IsRemoving));
        }

        private ICommand _setFieldCommand;

        public ICommand SetFieldCommand
        {
            get => _setFieldCommand ?? (_setFieldCommand = new RelayCommand(obj =>
            {
                InvokeIsReady();
            }, obj => CheckPlacedShips(ShipSizeValues)));
        }

        public void InvokeIsReady()
        {
            FieldVM.IsReady = true;
            FieldIsReadyEvent?.Invoke();
        }

        private bool SetShipPreview(Position headDeck)
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
                    ShipToPlace.HeadPosition = new Position(ShipToPlace.HeadPosition.Row + rowDifference, ShipToPlace.HeadPosition.Column);
                }
                if (shipDeck != null && shipDeck.Position.Column >= FieldVM.Size)
                {
                    int colDifference = (FieldVM.Size - 1) - shipDeck.Position.Column;
                    ShipToPlace.HeadPosition = new Position(ShipToPlace.HeadPosition.Row, ShipToPlace.HeadPosition.Column + colDifference);
                }
                return FieldVM.PreviewShip(ShipToPlace);
            }
            return false;
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

        private bool CheckPlacedShips(IEnumerable<ShipSize> shipSizeValues)
        {
            foreach (var size in shipSizeValues)
            {
                bool shipsChecked = false;
                switch (size)
                {
                    case ShipSize.Tiny:
                        shipsChecked = Ships.Where(ship => ship.ShipSize == size).Count() == GameRules.TinyShips;
                        break;
                    case ShipSize.Small:
                        shipsChecked = Ships.Where(ship => ship.ShipSize == size).Count() == GameRules.SmallShips;
                        break;
                    case ShipSize.Medium:
                        shipsChecked = Ships.Where(ship => ship.ShipSize == size).Count() == GameRules.MediumShips;
                        break;
                    case ShipSize.Large:
                        shipsChecked = Ships.Where(ship => ship.ShipSize == size).Count() == GameRules.LargeShips;
                        break;
                    default:
                        break;
                }
                if(!shipsChecked)
                {
                    return false;
                }
            };
            return true;
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
        public void RemoveShip(Position mousePosition)
        {
            var shipToRemove = FieldVM.GetShipByPosition(mousePosition);
            if(shipToRemove != null)
            {
                CurrentShipSize = shipToRemove.ShipSize;
                Ships.Remove(shipToRemove);
                CheckIfPossibleToPlace(CurrentShipSize);
            }
            
        }
    }
}
