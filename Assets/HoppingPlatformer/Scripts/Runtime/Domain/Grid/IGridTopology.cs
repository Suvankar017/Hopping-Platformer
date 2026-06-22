using System.Collections.Generic;
using HoppingPlatformer.Domain.Common;

namespace HoppingPlatformer.Domain.Grid
{
    public interface IGridTopology
    {
        int Width { get; }

        int GetRowWidth(int row);

        bool IsValid(HexPosition position);

        IReadOnlyList<HexPosition> GetUpNeighbours(HexPosition position);

        IReadOnlyList<HexPosition> GetDownNeighbours(HexPosition position);

        bool TryGetJumpTarget(HexPosition current, Direction direction, int distance, out HexPosition target);
    }
}