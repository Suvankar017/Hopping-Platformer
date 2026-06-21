using System.Collections.Generic;

public sealed class HexGridTopology
{
    private readonly int _width;

    public int Width => _width;

    public HexGridTopology(int width)
    {
        _width = width;
    }

    public int GetRowWidth(int row)
    {
        bool evenWidth = (_width & 1) == 0;
        int c = evenWidth ? 1 : 0;

        return ((row & 1) == c)
            ? _width
            : _width - 1;
    }

    public bool IsValid(HexPos pos)
    {
        if (pos.Row < 0)
            return false;

        int rowWidth = GetRowWidth(pos.Row);

        return pos.Col >= 0 &&
               pos.Col < rowWidth;
    }

    public List<HexPos> GetUpNeighbours(HexPos pos)
    {
        List<HexPos> result = new();

        int nextRow = pos.Row + 1;

        int currentWidth = GetRowWidth(pos.Row);
        int nextWidth = GetRowWidth(nextRow);

        bool currentLong = currentWidth > nextWidth;

        if (currentLong)
        {
            TryAdd(result, new HexPos(nextRow, pos.Col));
            TryAdd(result, new HexPos(nextRow, pos.Col - 1));
        }
        else
        {
            TryAdd(result, new HexPos(nextRow, pos.Col));
            TryAdd(result, new HexPos(nextRow, pos.Col + 1));
        }

        return result;
    }

    public List<HexPos> GetDownNeighbours(
    HexPos pos)
    {
        List<HexPos> result = new();

        if (pos.Row <= 0)
            return result;

        int previousRow = pos.Row - 1;

        int previousWidth = GetRowWidth(previousRow);

        int currentWidth = GetRowWidth(pos.Row);

        bool previousLong = previousWidth > currentWidth;

        if (previousLong)
        {
            TryAdd(result, new HexPos(previousRow, pos.Col));
            TryAdd(result, new HexPos(previousRow, pos.Col + 1));
        }
        else
        {
            TryAdd(result, new HexPos(previousRow, pos.Col));
            TryAdd(result, new HexPos(previousRow, pos.Col - 1));
        }

        return result;
    }

    //public bool TryGetLeftJump(
    //    HexPos current,
    //    out HexPos target)
    //{
    //    target = default;

    //    int nextRow = current.Row + 1;

    //    int currentWidth =
    //        GetRowWidth(current.Row);

    //    int nextWidth =
    //        GetRowWidth(nextRow);

    //    bool currentLong =
    //        currentWidth > nextWidth;

    //    if (currentLong)
    //    {
    //        target = new HexPos(
    //            nextRow,
    //            current.Col - 1);
    //    }
    //    else
    //    {
    //        target = new HexPos(
    //            nextRow,
    //            current.Col);
    //    }

    //    return IsValid(target);
    //}

    public bool TryGetLeftJump(
        HexPos current,
        int distance,
        out HexPos target)
    {
        target = current;

        for (int i = 0; i < distance; i++)
        {
            int nextRow = target.Row + 1;

            int currentWidth =
                GetRowWidth(target.Row);

            int nextWidth =
                GetRowWidth(nextRow);

            bool currentLong =
                currentWidth > nextWidth;

            HexPos next =
                currentLong
                    ? new HexPos(nextRow, target.Col - 1)
                    : new HexPos(nextRow, target.Col);

            target = next;

            if (!IsValid(next))
                return false;
        }

        return true;
    }

    //public bool TryGetRightJump(
    //    HexPos current,
    //    out HexPos target)
    //{
    //    target = default;

    //    int nextRow = current.Row + 1;

    //    int currentWidth =
    //        GetRowWidth(current.Row);

    //    int nextWidth =
    //        GetRowWidth(nextRow);

    //    bool currentLong =
    //        currentWidth > nextWidth;

    //    if (currentLong)
    //    {
    //        target = new HexPos(
    //            nextRow,
    //            current.Col);
    //    }
    //    else
    //    {
    //        target = new HexPos(
    //            nextRow,
    //            current.Col + 1);
    //    }

    //    return IsValid(target);
    //}

    public bool TryGetRightJump(
        HexPos current,
        int distance,
        out HexPos target)
    {
        target = current;

        for (int i = 0; i < distance; i++)
        {
            int nextRow = target.Row + 1;

            int currentWidth =
                GetRowWidth(target.Row);

            int nextWidth =
                GetRowWidth(nextRow);

            bool currentLong =
                currentWidth > nextWidth;

            HexPos next =
                currentLong
                    ? new HexPos(nextRow, target.Col)
                    : new HexPos(nextRow, target.Col + 1);

            target = next;

            if (!IsValid(next))
                return false;
        }

        return true;
    }

    private void TryAdd(
        List<HexPos> list,
        HexPos pos)
    {
        if (IsValid(pos))
            list.Add(pos);
    }
}
