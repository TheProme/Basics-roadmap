using SeaBattle.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.ViewModels
{
    public class PlayerAIViewModel : PlayerViewModel
    {
        public override event Action<IClickableCell, PlayerViewModel> FireEvent;
        public override bool PlayerTurn
        {
            get => base.PlayerTurn;
            set
            {
                base.PlayerTurn = value;
                if(value)
                {
                    CalculateShot();
                }
                OnPropertyChanged();
            }
        }


        private List<Position> _shots;
        private List<ShipBlockViewModel> _currentDamagedBlocks;
        private List<Position> _excludedPositions;
        private List<Position> _possibleShots;

        private async void CalculateShot(Position? lastHitCell = null)
        {
            await Task.Delay(1000);
            if(lastHitCell != null || _currentDamagedBlocks.Count() > 0)
            {
                var lastHittedBlock = _currentDamagedBlocks.Last();
                for (int i = lastHittedBlock.Position.Row - 1, rowIndex = 1; i < lastHittedBlock.Position.Row + 2; i++, rowIndex++)
                {
                    for (int j = lastHittedBlock.Position.Column - 1, colIndex = 1; j < lastHittedBlock.Position.Column + 2; j++, colIndex++)
                    {
                        Position emptyCell = new Position(i, j);
                        if(GameRules.CellMatchesGameField(emptyCell))
                        {
                            if (rowIndex % 2 > 0 && colIndex % 2 > 0)
                            {
                                if (emptyCell != lastHittedBlock.Position &&
                                    !_excludedPositions.Contains(emptyCell))
                                {
                                    _excludedPositions.Add(emptyCell);
                                }
                            }
                            else
                            {
                                _possibleShots.Add(emptyCell);
                            }
                        }
                    }
                }
                var nextShot = _possibleShots.Except(_excludedPositions).Except(_shots).ToArray();
                Fire(nextShot[rnd.Next(nextShot.Length)]);
            }
            else
            {
                Fire(GetRandomCellToFire());
            }
        }

        private void Fire(Position cell)
        {
            _shots.Add(cell);
            var hitCell = OpponentField.FieldVM.FieldCells.FirstOrDefault(c => c.Position == cell);
            if (hitCell != null)
            {
                hitCell.CellValue.InvokeHit();
                FireEvent?.Invoke(hitCell.CellValue, this);
            }
        }

        public void AddTurn(ShipBlockViewModel damagedBlock)
        {
            if(!damagedBlock.ShipBase.Destroyed)
            {
                _currentDamagedBlocks.Add(damagedBlock);
                CalculateShot(damagedBlock.Position);
            }
            else
            {
                _currentDamagedBlocks.Clear();
                _possibleShots.Clear();
                foreach (var item in _excludedPositions)
                {
                    if(!_shots.Exists(shot => shot == item))
                        _shots.Add(item);
                }
                CalculateShot();
            }
            
        }

        private Position GetRandomCellToFire()
        {
            var newShot = new Position(rnd.Next(GameRules.FieldSize), rnd.Next(GameRules.FieldSize));
            while (_shots.Exists(shot => shot == newShot))
            {
                newShot = new Position(rnd.Next(GameRules.FieldSize), rnd.Next(GameRules.FieldSize));
            }
            return newShot;
        }

        public void SetAIReady()
        {
            FieldPreview.FieldVM.IsReady = true;
            FieldPreview.FieldVM.CanClick = true;
        }


        private Random rnd = new Random();

        public PlayerAIViewModel(SetupFieldViewModel previewField, SetupFieldViewModel opponentField) : base(previewField, opponentField)
        {
            _shots = new List<Position>();
            _currentDamagedBlocks = new List<ShipBlockViewModel>();
            _excludedPositions = new List<Position>();
            _possibleShots = new List<Position>();
        }
    }
}
