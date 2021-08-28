using SeaBattle.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace SeaBattle.ViewModels
{
    public class PlayerViewModel : BaseViewModel
    {
        public event Action<PlayerViewModel> PlayerReadyEvent;
        public virtual event Action<IClickableCell, PlayerViewModel> FireEvent;

        private SetupFieldViewModel _fieldPreview;

        public SetupFieldViewModel FieldPreview
        {
            get => _fieldPreview;
            set 
            { 
                _fieldPreview = value;
                OnPropertyChanged();
            }
        }

        private SetupFieldViewModel _opponentField;

        public SetupFieldViewModel OpponentField
        {
            get => _opponentField; 
            private set 
            { 
                _opponentField = value;
                OnPropertyChanged();
            }
        }

        private bool _playerTurn;

        public virtual bool PlayerTurn
        {
            get => _playerTurn;
            set 
            { 
                _playerTurn = value;
                OnPropertyChanged();
            }
        }

        private bool _fieldIsSet = false;

        public bool FieldIsSet
        {
            get => _fieldIsSet; 
            set 
            { 
                _fieldIsSet = value;
                OnPropertyChanged();
            }
        }


        public ObservableCollection<ShipViewModel> ActiveShips => FieldPreview.Ships;

        public PlayerViewModel(SetupFieldViewModel myField, SetupFieldViewModel opponentField)
        {
            FieldPreview = myField;
            FieldPreview.FieldIsReadyEvent += FieldIsReadyHandler;
            OpponentField = opponentField;
        }

        private void FieldShotHandler(IClickableCell obj)
        {
            FireEvent?.Invoke(obj, this);
        }

        private void FieldIsReadyHandler()
        {
            if(OpponentField != null)
            {
                OpponentField.FieldVM.ShotEvent -= FieldShotHandler;
            }
            FieldIsSet = true;
            OpponentField.FieldVM.ShotEvent += FieldShotHandler;
            PlayerReadyEvent?.Invoke(this);
        }
        public bool AreShipsDestroyed()
        {
            return ActiveShips.All(ship => ship.Destroyed);
        }

        public virtual void ClearField()
        {
            FieldPreview.FieldVM.ClearField();
        }
    }
}
