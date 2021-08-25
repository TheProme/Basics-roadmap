using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle.Extensions
{
    public struct Position
    {
        public int Row { get; private set; }
        public int Column { get; private set; }

        public Position(int row, int col)
        {
            Row = row;
            Column = col;
        }
        public static bool operator ==(Position left, Position right) => left.Row == right.Row && left.Column == right.Column;
        public static bool operator !=(Position left, Position right) => !(left == right);
        public override bool Equals(object obj)
        {
            if((obj as Position?).HasValue)
            {
                return this == (Position)obj;
            }
            return false;
        }
        public override int GetHashCode() => Row.GetHashCode() ^ Column.GetHashCode();
        public override string ToString()
        {
            return $"[{Row},{Column}]";
        }
    }
}
