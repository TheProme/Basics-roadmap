using SeaBattle.Commands;
using SeaBattle.Extensions;
using System;
using System.Windows;
using System.Windows.Input;

namespace SeaBattle.ViewModels
{
    public class FieldCellViewModel : BaseViewModel
    {
        private static readonly int _defaultCellSize = 30;

        private int _cellSize = _defaultCellSize;

        public int CellSize
        {
            get => _cellSize;
            set 
            { 
                _cellSize = value;
                OnPropertyChanged();
            }
        }


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

        private IClickableCell _cellValue;

        public IClickableCell CellValue
        {
            get => _cellValue;
            set 
            { 
                _cellValue = value;
                OnPropertyChanged();
            }
        }

        private bool _isPreview = false;

        public bool IsPreview
        {
            get => _isPreview;
            set 
            { 
                _isPreview = value;
                OnPropertyChanged();
            }
        }

        private bool _isOccupied;

        public bool IsOccupied
        {
            get => _isOccupied;
            set 
            { 
                _isOccupied = value;
                OnPropertyChanged();
            }
        }

        public FieldCellViewModel(Position position, IClickableCell cellValue)
        {
            Position = position;
            CellValue = cellValue;
        }
    }
}
