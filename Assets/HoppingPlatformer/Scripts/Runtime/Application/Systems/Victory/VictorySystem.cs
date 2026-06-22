using HoppingPlatformer.Application.Events;

using HoppingPlatformer.Domain.Level;
using HoppingPlatformer.Domain.Player;

namespace HoppingPlatformer.Application.Systems.Victory
{
    public sealed class VictorySystem : IVictorySystem
    {
        private readonly Player _player;

        private readonly Level _level;

        private readonly IEventBus _eventBus;

        public VictorySystem(Player player, Level level, IEventBus eventBus)
        {
            _player = player;
            _level = level;
            _eventBus = eventBus;

            _eventBus.Subscribe<PlayerLandedEvent>(OnPlayerLanded);
        }

        private void OnPlayerLanded(PlayerLandedEvent gameEvent)
        {
            CheckVictory();
        }

        public bool CheckVictory()
        {
            if (!_level.TryGetPlatform(_player.Position, out Platform platform))
            {
                return false;
            }

            if (platform.Type != PlatformType.Goal)
            {
                return false;
            }

            _player.ChangeState(PlayerState.Victory);

            _eventBus.Publish(new PlayerWonEvent());

            return true;
        }
    }
}