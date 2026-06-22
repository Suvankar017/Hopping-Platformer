using HoppingPlatformer.Domain.Common;
using HoppingPlatformer.Domain.Level;

namespace HoppingPlatformer.Application.Systems.GoalPath
{
    public interface IGoalPathService
    {
        Domain.Level.GoalPath BuildPath(HexPosition start);
    }
}