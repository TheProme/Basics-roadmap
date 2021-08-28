using SeaBattle.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle.Extensions
{
    public static class GameRules
    {
        public static readonly int TinyShips = 4;
        public static readonly int SmallShips = 3;
        public static readonly int MediumShips = 2;
        public static readonly int LargeShips = 1;

        public static readonly int FieldCellsCount = 10;

        public static readonly int DefaultCellSize = 30;

        public static bool CellMatchesGameField(Position cellPosition)
        {
            if(cellPosition.Row >= 0 &&
               cellPosition.Row < GameRules.FieldCellsCount &&
               cellPosition.Column >= 0 &&
               cellPosition.Column < GameRules.FieldCellsCount)
            {
                return true;
            }
            return false;
        }
    }
}
