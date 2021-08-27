using SeaBattle.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace SeaBattle.ViewModels
{
    public class EmptyCellViewModel : BaseViewModel, IClickableCell
    {
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

        public event Action<IClickableCell> HitEvent;

        private bool _isHit;

        public bool IsHit
        {
            get => _isHit;
            set
            {
                _isHit = value;
                if (_isHit && !_openNeighbours)
                {
                    HitEvent?.Invoke(this);
                }
                OnPropertyChanged();
            }
        }

        private bool _openNeighbours;

        public void InvokeHit(bool openNeighbours = false)
        {
            _openNeighbours = openNeighbours;
            IsHit = true;
        }

        public EmptyCellViewModel(Position position)
        {
            Position = position;
        }
    }
}
