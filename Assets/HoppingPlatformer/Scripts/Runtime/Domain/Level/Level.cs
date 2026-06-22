using System.Collections.Generic;
using HoppingPlatformer.Domain.Common;

namespace HoppingPlatformer.Domain.Level
{
    public sealed class Level
    {
        private readonly Dictionary<HexPosition, Platform> _platforms;

        public HexPosition Start { get; }

        public HexPosition Goal { get; }

        public IReadOnlyDictionary<HexPosition, Platform> Platforms => _platforms;

        public Level(Dictionary<HexPosition, Platform> platforms, HexPosition start, HexPosition goal)
        {
            _platforms = platforms;

            Start = start;

            Goal = goal;
        }

        public bool TryGetPlatform(HexPosition position, out Platform platform)
        {
            return _platforms.TryGetValue(position, out platform);
        }

        public bool ContainsPlatform(HexPosition position)
        {
            return _platforms.ContainsKey(position);
        }
    }
}
