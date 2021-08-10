using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Extensions
{
    public struct Position
    {
        public int Row { get; }
        public int Column { get; }

        public Position(int row, int col)
        {
            Row = row;
            Column = col;
        }

        public static bool operator == (Position left, Position right) => left.Row == right.Row && left.Column == right.Column;
        public static bool operator != (Position left, Position right) => !(left == right);
        public override bool Equals(object obj) => this == (Position)obj;
        public override int GetHashCode() => Row.GetHashCode() ^ Column.GetHashCode();
    }
}
