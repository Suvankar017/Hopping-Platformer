using HoppingPlatformer.Domain.Common;

namespace HoppingPlatformer.Application.Systems.Movement
{
    public interface IMovementSystem
    {
        MovementResult Move(Direction direction);
    }
}