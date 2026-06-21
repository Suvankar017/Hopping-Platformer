using UnityEngine;

[System.Serializable]
public sealed class LevelGenerationSettings
{
    [Header("Density")]
    public AnimationCurve DensityCurve =
        AnimationCurve.Linear(0f, 1f, 1f, 0.25f);

    [Header("Branches")]
    public int BaseAlternateBranches = 2;

    [Tooltip("Additional branches per floor.")]
    public float AlternateBranchesPerFloor = 0.15f;

    [Header("Rewards")]
    public int BaseRewardBranches = 1;

    [Tooltip("Additional reward branches per floor.")]
    public float RewardBranchesPerFloor = 0.05f;

    [Header("Lengths")]
    public int MinBranchLength = 2;
    public int MaxBranchLength = 4;

    [Tooltip("Extra max length based on level height.")]
    public float BranchLengthScale = 0.03f;

    [Header("Items")]
    public float CoinChance = 0.25f;

    public float GemChance = 0.7f;
}