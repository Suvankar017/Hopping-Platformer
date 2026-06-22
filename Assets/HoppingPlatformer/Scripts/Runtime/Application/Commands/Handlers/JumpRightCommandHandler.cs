using HoppingPlatformer.Domain.Common;

using HoppingPlatformer.Application.Systems.Movement;

namespace HoppingPlatformer.Application.Commands
{
    public sealed class JumpRightCommandHandler : ICommandHandler<JumpRightCommand>
    {
        private readonly IMovementSystem _movementSystem;

        public JumpRightCommandHandler(IMovementSystem movementSystem)
        {
            _movementSystem = movementSystem;
        }

        public void Handle(JumpRightCommand command)
        {
            _movementSystem.Move(Direction.Right);
        }
    }
}