using HoppingPlatformer.Domain.Common;

namespace HoppingPlatformer.Application.Systems.Movement
{
    public readonly struct MovementResult
    {
        public bool Success { get; }

        public HexPosition From { get; }

        public HexPosition To { get; }

        public MovementResult(bool success, HexPosition from, HexPosition to)
        {
            Success = success;
            From = from;
            To = to;
        }
    }
}