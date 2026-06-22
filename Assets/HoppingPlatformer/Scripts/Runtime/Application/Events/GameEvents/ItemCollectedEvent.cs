using HoppingPlatformer.Domain.Level;

namespace HoppingPlatformer.Application.Events
{
    public readonly struct ItemCollectedEvent : IGameEvent
    {
        public ItemType Item { get; }

        public ItemCollectedEvent(ItemType item)
        {
            Item = item;
        }
    }
}