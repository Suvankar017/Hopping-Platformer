using System.Collections.Generic;

using HoppingPlatformer.Domain.Common;
using HoppingPlatformer.Domain.Level;

namespace HoppingPlatformer.Application.Systems.GoalPath
{
    public sealed class GoalPathService : IGoalPathService
    {
        private readonly Level _level;

        public GoalPathService(Level level)
        {
            _level = level;
        }

        public Domain.Level.GoalPath BuildPath(HexPosition start)
        {
            List<HexPosition> positions = new List<HexPosition>();

            if (!_level.TryGetPlatform(start, out Platform current))
            {
                return new Domain.Level.GoalPath(positions);
            }

            positions.Add(start);

            while (current.CanReachGoal)
            {
                HexPosition next = current.NextTowardsGoal;

                positions.Add(next);

                if (!_level.TryGetPlatform(next, out current))
                {
                    break;
                }
            }

            return new Domain.Level.GoalPath(positions);
        }
    }
}