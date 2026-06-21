using System;

[Serializable]
public readonly struct HexPos : IEquatable<HexPos>
{
    public readonly int Row;
    public readonly int Col;

    public HexPos(int row, int col)
    {
        Row = row;
        Col = col;
    }

    public bool Equals(HexPos other)
        => Row == other.Row && Col == other.Col;

    public override bool Equals(object obj)
        => obj is HexPos other && Equals(other);

    public override int GetHashCode()
        => HashCode.Combine(Row, Col);

    public override string ToString()
        => $"({Row}, {Col})";
}
