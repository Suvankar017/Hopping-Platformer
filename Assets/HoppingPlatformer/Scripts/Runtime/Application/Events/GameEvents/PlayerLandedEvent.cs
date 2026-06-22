using HoppingPlatformer.Domain.Common;

namespace HoppingPlatformer.Application.Events
{
    public readonly struct PlayerLandedEvent : IGameEvent
    {
        public HexPosition Position { get; }

        public PlayerLandedEvent(HexPosition position)
        {
            Position = position;
        }
    }
}