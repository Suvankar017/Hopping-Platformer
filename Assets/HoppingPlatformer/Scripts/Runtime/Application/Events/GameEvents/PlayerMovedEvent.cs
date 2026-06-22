using HoppingPlatformer.Domain.Common;

namespace HoppingPlatformer.Application.Events
{
    public readonly struct PlayerMovedEvent : IGameEvent
    {
        public HexPosition From { get; }

        public HexPosition To { get; }

        public PlayerMovedEvent(HexPosition from, HexPosition to)
        {
            From = from;
            To = to;
        }
    }
}