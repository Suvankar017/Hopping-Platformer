using System.Collections.Generic;

using HoppingPlatformer.Domain.Common;
using HoppingPlatformer.Domain.Level;

namespace HoppingPlatformer.Domain.Generation
{
    public sealed class GenerationContext
    {
        public Dictionary<HexPosition, Platform> Platforms { get; }

        public List<HexPosition> MainPath { get; }

        public HexPosition Start { get; set; }

        public HexPosition Goal { get; set; }

        public int Height { get; }

        public GenerationContext(int height)
        {
            Height = height;

            Platforms = new Dictionary<HexPosition, Platform>();

            MainPath = new List<HexPosition>();
        }
    }
}