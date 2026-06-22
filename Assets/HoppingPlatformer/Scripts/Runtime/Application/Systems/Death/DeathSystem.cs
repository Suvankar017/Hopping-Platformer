using HoppingPlatformer.Application.Events;

using HoppingPlatformer.Domain.Player;

namespace HoppingPlatformer.Application.Systems.Death
{
    public sealed class DeathSystem : IDeathSystem
    {
        private readonly Player _player;

        private readonly IEventBus _eventBus;

        public DeathSystem(Player player, IEventBus eventBus)
        {
            _player = player;
            _eventBus = eventBus;
        }

        public void KillPlayer()
        {
            _player.ChangeState(PlayerState.Dead);

            _eventBus.Publish(new PlayerDiedEvent());
        }
    }
}