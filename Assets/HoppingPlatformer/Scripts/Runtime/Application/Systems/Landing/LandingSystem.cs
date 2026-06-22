using HoppingPlatformer.Application.Events;

namespace HoppingPlatformer.Application.Systems.Landing
{
    public sealed class LandingSystem : ILandingSystem
    {
        private readonly IEventBus _eventBus;

        public LandingSystem(IEventBus eventBus)
        {
            _eventBus = eventBus;

            _eventBus.Subscribe<PlayerMovedEvent>(OnPlayerMoved);
        }

        private void OnPlayerMoved(PlayerMovedEvent gameEvent)
        {
            _eventBus.Publish(new PlayerLandedEvent(gameEvent.To));
        }
    }
}