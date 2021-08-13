using SeaBattle.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle.ViewModels
{
    public class ShipBlockViewModel : BaseViewModel
    {
        private ShipViewModel _shipBase;

        public ShipViewModel ShipBase
        {
            get => _shipBase;
            private set 
            { 
                _shipBase = value;
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

        private int _blockSize;

        public int BlockSize
        {
            get => _blockSize;
            set 
            { 
                _blockSize = value;
                OnPropertyChanged();
            }
        }


        public event Action BlockHitEvent;

        private bool _isHit;

        public bool IsHit
        {
            get => _isHit;
            set 
            { 
                _isHit = value;
                if (_isHit)
                {
                    InvokeHitEvent();
                }
                OnPropertyChanged();
            }
        }

        public ShipBlockViewModel(ShipViewModel parentShip)
        {
            ShipBase = parentShip;
        }

        public void InvokeHitEvent()
        {
            BlockHitEvent?.Invoke();
        }
    }
}
