using System.Collections.Generic;
using HoppingPlatformer.Domain.Common;

namespace HoppingPlatformer.Domain.Level
{
    public sealed class GoalPath
    {
        public IReadOnlyList<HexPosition> Positions { get; }

        public GoalPath(IReadOnlyList<HexPosition> positions)
        {
            Positions = positions;
        }
    }
}
