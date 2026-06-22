using HoppingPlatformer.Domain.Common;

namespace HoppingPlatformer.Domain.Level
{
    public sealed class Platform
    {
        public HexPosition Position { get; }

        public PlatformType Type { get; private set; }

        public ItemType Item { get; private set; }

        public bool CanReachGoal { get; private set; }

        public HexPosition NextTowardsGoal { get; private set; }

        public Platform(HexPosition position, PlatformType type = PlatformType.Normal)
        {
            Position = position;
            Type = type;
            Item = ItemType.None;
        }

        public void SetType(PlatformType type)
        {
            Type = type;
        }

        public void SetItem(ItemType item)
        {
            Item = item;
        }

        public void MarkReachable(HexPosition nextPosition)
        {
            CanReachGoal = true;
            NextTowardsGoal = nextPosition;
        }
    }
}
