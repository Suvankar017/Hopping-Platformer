namespace HoppingPlatformer.Domain.Generation
{
    public sealed class LevelGenerationSettings
    {
        public int BaseAlternateBranches { get; set; }

        public int BaseRewardBranches { get; set; }

        public float BranchLengthScale { get; set; }

        public float AlternateBranchesPerFloor { get; set; }

        public int MinBranchLength { get; set; }

        public int MaxBranchLength { get; set; }

        public float CoinChance { get; set; }

        public float GemChance { get; set; }
    }
}