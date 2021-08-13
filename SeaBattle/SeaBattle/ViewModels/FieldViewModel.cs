using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

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


        private int _size;

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

        public List<FieldCellViewModel> FieldCells { get; set; } = new List<FieldCellViewModel>();

        public ObservableCollection<ShipViewModel> Ships { get; set; } = new ObservableCollection<ShipViewModel>();


        public FieldViewModel(ObservableCollection<ShipViewModel> ships, int size)
        {
            Ships = ships;
            Size = size;
        }

        private void CreateField(int size)
        {
            if (FieldCells.Count > 0)
            {
                FieldCells.Clear();
            }
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    foreach (var ship in Ships)
                    {
                        if(ship.ShipBlocks.Any(block => block.Position.Row == i && block.Position.Column == j))
                        {
                            var existingBlock = ship.ShipBlocks.FirstOrDefault(block => block.Position.Row == i && block.Position.Column == j);
                            if(existingBlock != null)
                            {
                                var shipCell = new FieldCellViewModel(existingBlock.Position, existingBlock);
                                shipCell.Clicked += BlockIsHit;
                                FieldCells.Add(shipCell);
                            }
                        }
                    }
                    if(!FieldCells.Exists(cell=> cell.Position.Row == i && cell.Position.Column == j))
                    {
                        var cell = new FieldCellViewModel(new Extensions.Position(i, j));
                        cell.Clicked += BlockIsHit;
                        FieldCells.Add(cell);
                    }
                }
            }
        }

        private void BlockIsHit(object cellValue)
        {
            if(cellValue is ShipBlockViewModel)
            {
                (cellValue as ShipBlockViewModel).InvokeHitEvent();
            }
        }
    }
}
