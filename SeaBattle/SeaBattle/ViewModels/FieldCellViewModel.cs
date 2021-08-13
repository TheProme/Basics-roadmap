using SeaBattle.Extensions;
using System;

namespace SeaBattle.ViewModels
{
    public class FieldCellViewModel : BaseViewModel
    {
        public delegate void CellClicked(object cellValue);
        public event CellClicked Clicked;

        private Position _position;

        public Position Position
        {
            get => _position;
            set 
            { 
                _position = value;
                OnPropertyChanged();
            }
        }

        private bool _isActive;

        public bool IsActive
        {
            get => _isActive;
            set 
            { 
                _isActive = value;
                OnPropertyChanged();
            }
        }

        private object _cellValue;

        public object CellValue
        {
            get => _cellValue;
            set 
            { 
                _cellValue = value;
                OnPropertyChanged();
            }
        }

        private ShipViewModel _parentModel;

        public ShipViewModel ParentModel
        {
            get { return _parentModel; }
            private set { _parentModel = value; }
        }



        public FieldCellViewModel(Position position, object cellValue = null)
        {
            Position = position;
            CellValue = cellValue;
        }

    }
}
