using System;
using System.Collections.Generic;
using HoppingPlatformer.Domain.Common;

namespace HoppingPlatformer.Domain.Grid
{
    public sealed class HexGridTopology : IGridTopology
    {
        private readonly int _width;

        public int Width => _width;

        public HexGridTopology(int width)
        {
            if (width <= 1)
            {
                throw new ArgumentOutOfRangeException(nameof(width),
                    "Width must be greater than 1.");
            }

            _width = width;
        }

        public int GetRowWidth(int row)
        {
            bool evenWidth = (_width & 1) == 0;

            int parity = evenWidth ? 1 : 0;

            return ((row & 1) == parity) ? _width : _width - 1;
        }

        public bool IsValid(HexPosition position)
        {
            if (position.Row < 0)
            {
                return false;
            }

            int rowWidth = GetRowWidth(position.Row);

            return position.Column >= 0 && position.Column < rowWidth;
        }

        public IReadOnlyList<HexPosition> GetUpNeighbours(HexPosition position)
        {
            List<HexPosition> neighbours = new(2);

            int nextRow = position.Row + 1;

            int currentWidth = GetRowWidth(position.Row);

            int nextWidth = GetRowWidth(nextRow);

            bool currentLonger = currentWidth > nextWidth;

            if (currentLonger)
            {
                TryAdd(neighbours, new HexPosition(nextRow, position.Column));

                TryAdd(neighbours, new HexPosition(nextRow, position.Column - 1));
            }
            else
            {
                TryAdd(neighbours, new HexPosition(nextRow, position.Column));

                TryAdd(neighbours, new HexPosition(nextRow, position.Column + 1));
            }

            return neighbours;
        }

        public IReadOnlyList<HexPosition> GetDownNeighbours(HexPosition position)
        {
            List<HexPosition> neighbours = new(2);

            if (position.Row <= 0)
            {
                return neighbours;
            }

            int previousRow = position.Row - 1;

            int previousWidth = GetRowWidth(previousRow);

            int currentWidth = GetRowWidth(position.Row);

            bool previousLonger = previousWidth > currentWidth;

            if (previousLonger)
            {
                TryAdd(neighbours, new HexPosition(previousRow, position.Column));

                TryAdd(neighbours, new HexPosition(previousRow, position.Column + 1));
            }
            else
            {
                TryAdd(neighbours, new HexPosition(previousRow, position.Column));

                TryAdd(neighbours, new HexPosition(previousRow, position.Column - 1));
            }

            return neighbours;
        }

        public bool TryGetJumpTarget(HexPosition current, Direction direction, int distance, out HexPosition target)
        {
            target = current;

            if (distance <= 0)
            {
                return false;
            }

            for (int i = 0; i < distance; i++)
            {
                int nextRow = target.Row + 1;

                int currentWidth = GetRowWidth(target.Row);

                int nextWidth = GetRowWidth(nextRow);

                bool currentLonger = currentWidth > nextWidth;

                HexPosition next;

                switch (direction)
                {
                    case Direction.Left:

                        next = currentLonger
                            ? new HexPosition(nextRow, target.Column - 1)
                            : new HexPosition(nextRow, target.Column);

                        break;

                    case Direction.Right:

                        next = currentLonger
                            ? new HexPosition(nextRow, target.Column)
                            : new HexPosition(nextRow, target.Column + 1);

                        break;

                    default:
                        return false;
                }

                if (!IsValid(next))
                {
                    return false;
                }

                target = next;
            }

            return true;
        }

        private void TryAdd(ICollection<HexPosition> collection, HexPosition position)
        {
            if (IsValid(position))
            {
                collection.Add(position);
            }
        }
    }
}