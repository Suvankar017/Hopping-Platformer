using HoppingPlatformer.Application.Events;

using HoppingPlatformer.Domain.Level;
using HoppingPlatformer.Domain.Player;

namespace HoppingPlatformer.Application.Systems.Interaction
{
    public sealed class InteractionSystem : IInteractionSystem
    {
        private readonly Player _player;

        private readonly Level _level;

        private readonly IEventBus _eventBus;

        public InteractionSystem(Player player, Level level, IEventBus eventBus)
        {
            _player = player;
            _level = level;
            _eventBus = eventBus;

            _eventBus.Subscribe<PlayerLandedEvent>(OnPlayerLanded);
        }

        private void OnPlayerLanded(PlayerLandedEvent gameEvent)
        {
            ResolveCurrentPlatform();
        }

        public void ResolveCurrentPlatform()
        {
            if (!_level.TryGetPlatform(_player.Position, out Platform platform))
            {
                return;
            }

            switch (platform.Item)
            {
                case ItemType.None:
                    break;

                case ItemType.Coin:
                    ResolveCoin(platform);
                    break;

                case ItemType.DoubleJump:
                    ResolveDoubleJump(platform);
                    break;

                case ItemType.JumpBooster:
                    ResolveJumpBooster(platform);
                    break;
            }
        }

        private void ResolveCoin(Platform platform)
        {
            _player.Wallet.AddCoin();

            platform.SetItem(ItemType.None);

            _eventBus.Publish(new CoinCollectedEvent(_player.Wallet.Coins));

            _eventBus.Publish(new ItemCollectedEvent(ItemType.Coin));
        }

        private void ResolveDoubleJump(Platform platform)
        {
            _player.Jump.EnableDoubleJump();

            platform.SetItem(ItemType.None);

            _eventBus.Publish(new AbilityGrantedEvent("DoubleJump"));

            _eventBus.Publish(new ItemCollectedEvent(ItemType.DoubleJump));
        }

        private void ResolveJumpBooster(Platform platform)
        {
            platform.SetItem(ItemType.None);

            _eventBus.Publish(new ItemCollectedEvent(ItemType.JumpBooster));
        }
    }
}