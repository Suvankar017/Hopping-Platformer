using HoppingPlatformer.Application.Events;
using HoppingPlatformer.Application.Systems.Death;

using HoppingPlatformer.Domain.Common;
using HoppingPlatformer.Domain.Grid;
using HoppingPlatformer.Domain.Level;
using HoppingPlatformer.Domain.Player;

namespace HoppingPlatformer.Application.Systems.Movement
{
    public sealed class MovementSystem : IMovementSystem
    {
        private readonly Player _player;

        private readonly Level _level;

        private readonly IGridTopology _topology;

        private readonly IEventBus _eventBus;

        private readonly IDeathSystem _deathSystem;

        public MovementSystem(Player player, Level level, IGridTopology topology, IEventBus eventBus, IDeathSystem deathSystem)
        {
            _player = player;
            _level = level;
            _topology = topology;
            _eventBus = eventBus;
            _deathSystem = deathSystem;
        }

        public MovementResult Move(Direction direction)
        {
            HexPosition current = _player.Position;

            bool valid = _topology.TryGetJumpTarget(current, direction, _player.Jump.Distance, out HexPosition target);

            if (!valid)
            {
                return new MovementResult(false, current, current);
            }

            if (!_level.ContainsPlatform(target))
            {
                _deathSystem.KillPlayer();

                return new MovementResult(false, current, target);
            }

            _player.MoveTo(target);

            if (_player.Jump.IsEnhanced)
            {
                _player.Jump.Consume();
            }

            _eventBus.Publish(new PlayerMovedEvent(current, target));

            return new MovementResult(true, current, target);
        }
    }
}