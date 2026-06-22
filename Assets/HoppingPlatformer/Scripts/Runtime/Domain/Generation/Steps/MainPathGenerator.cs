using System.Collections.Generic;

using HoppingPlatformer.Domain.Common;
using HoppingPlatformer.Domain.Grid;
using HoppingPlatformer.Domain.Level;

namespace HoppingPlatformer.Domain.Generation
{
    public sealed class MainPathGenerator : IGenerationStep
    {
        private readonly IGridTopology _topology;

        private readonly IRandom _random;

        public MainPathGenerator(IGridTopology topology, IRandom random)
        {
            _topology = topology;
            _random = random;
        }

        public void Execute(GenerationContext context)
        {
            HexPosition current = context.Start;

            context.MainPath.Add(current);

            while (current.Row < context.Height - 1)
            {
                IReadOnlyList<HexPosition> neighbours = _topology.GetUpNeighbours(current);

                current = neighbours[_random.Range(0, neighbours.Count)];

                context.MainPath.Add(current);

                if (!context.Platforms.ContainsKey(current))
                {
                    context.Platforms.Add(current, new Platform(current));
                }
            }

            context.Goal = current;

            context.Platforms[context.Goal].SetType(PlatformType.Goal);
        }
    }
}