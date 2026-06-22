using System;

namespace HoppingPlatformer.Domain.Common
{
    public readonly struct HexPosition : IEquatable<HexPosition>
    {
        public readonly int Row;
        public readonly int Column;

        public HexPosition(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public bool Equals(HexPosition other)
        {
            return Row == other.Row && Column == other.Column;
        }

        public override bool Equals(object obj)
        {
            return obj is HexPosition other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Column);
        }

        public override string ToString()
        {
            return $"({Row}, {Column})";
        }

        public static bool operator ==(HexPosition left, HexPosition right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(HexPosition left, HexPosition right)
        {
            return !left.Equals(right);
        }
    }
}
