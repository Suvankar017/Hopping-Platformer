using System.Collections.Generic;
using UnityEngine;

public sealed class LevelRuntime
{
    public readonly Dictionary<HexPos, PlatformView> Platforms;

    public readonly List<HexPos> MainPath;

    public readonly HexGridTopology Topology;

    public readonly HexPos Start;
    public readonly HexPos Goal;

    private readonly LevelData _levelData;

    public LevelRuntime(
        Dictionary<HexPos, PlatformView> platforms,
        HexGridTopology topology,
        HexPos start,
        HexPos goal,
        List<HexPos> mainPath,
        LevelData levelData)
    {
        Platforms = platforms;
        Topology = topology;

        Start = start;
        Goal = goal;

        MainPath = mainPath;

        _levelData = levelData;
    }

    public bool TryGetPlatform(
        HexPos position,
        out PlatformView platform)
    {
        return Platforms.TryGetValue(
            position,
            out platform);
    }

    public bool TryGetReachablePlatform(
        int row,
        out PlatformView platform)
    {
        platform = null;

        foreach (var pair in Platforms)
        {
            PlatformView candidate =
                pair.Value;

            if (candidate.Node.Position.Row != row)
                continue;

            if (!candidate.Node.CanReachGoal)
                continue;

            platform = candidate;

            return true;
        }

        return false;
    }

    public bool TryGetReachablePlatform(
        int row,
        int preferredColumn,
        out PlatformView platform)
    {
        platform = null;
        int minDistance = int.MaxValue;

        foreach (var pair in Platforms)
        {
            PlatformView candidate =
                pair.Value;

            if (candidate.Node.Position.Row != row)
                continue;

            if (!candidate.Node.CanReachGoal)
                continue;

            int distance = Mathf.Abs(candidate.Node.Position.Col - preferredColumn);
            if (distance < minDistance)
            {
                minDistance = distance;
                platform = candidate;
            }
        }

        return platform != null;
    }

    public bool TryGetSkipTarget(
        PlatformView current,
        int skipAmount,
        out PlatformView target)
    {
        int landingRow =
            current.Node.Position.Row +
            skipAmount +
            1;

        return TryGetReachablePlatform(
            landingRow,
            current.Node.Position.Col,
            out target);
    }

    public List<HexPos> GetPathToGoal(
        HexPos start)
    {
        return _levelData.GetPathToGoal(start);
    }
}
