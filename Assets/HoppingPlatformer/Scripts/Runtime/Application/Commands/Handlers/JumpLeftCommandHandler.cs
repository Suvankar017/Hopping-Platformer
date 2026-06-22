using HoppingPlatformer.Domain.Common;

using HoppingPlatformer.Application.Systems.Movement;

namespace HoppingPlatformer.Application.Commands
{
    public sealed class JumpLeftCommandHandler : ICommandHandler<JumpLeftCommand>
    {
        private readonly IMovementSystem _movementSystem;

        public JumpLeftCommandHandler(IMovementSystem movementSystem)
        {
            _movementSystem = movementSystem;
        }

        public void Handle(JumpLeftCommand command)
        {
            _movementSystem.Move(Direction.Left);
        }
    }
}