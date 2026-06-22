using HoppingPlatformer.Domain.Common;
using HoppingPlatformer.Domain.Grid;
using System;

namespace HoppingPlatformer.Domain.Generation
{
    public sealed class AlternateBranchGenerator : IGenerationStep
    {
        private readonly IGridTopology _topology;

        private readonly IRandom _random;

        private readonly LevelGenerationSettings _settings;

        public AlternateBranchGenerator(IGridTopology topology, IRandom random, LevelGenerationSettings settings)
        {
            _topology = topology;
            _random = random;
            _settings = settings;
        }

        public void Execute(GenerationContext context)
        {
            int height = context.Height;
            int extraLength = (int)MathF.Floor(height * _settings.BranchLengthScale);

            int maxLength = _settings.MaxBranchLength + extraLength;

            int branchCount = _settings.BaseAlternateBranches + (int)MathF.Round(height * _settings.AlternateBranchesPerFloor);

            var mainPath = context.MainPath;

            for (int i = 0; i < branchCount; i++)
            {
                int startIndex = _random.Range(0, mainPath.Count - 3);

                HexPosition current = mainPath[startIndex];

                int branchLength = _random.Range(_settings.MinBranchLength, maxLength + 1);

                for (int step = 0; step < branchLength; step++)
                {
                    if (current.Row >= height - 2)
                        break;

                    float t = current.Row / (float)(height - 1);

                    //float density = _settings.DensityCurve.Evaluate(t);

                    //if (Random.value > density)
                    //    break;

                    //List<HexPos> options =
                    //    _topology.GetUpNeighbours(current);

                    //if (options.Count == 0)
                    //    break;

                    //HexPos next =
                    //    options[
                    //        Random.Range(
                    //            0,
                    //            options.Count)];

                    //if (!nodes.ContainsKey(next))
                    //{
                    //    nodes[next] =
                    //        new PlatformNode(next);
                    //}

                    //current = next;
                }
            }
        }
    }
}