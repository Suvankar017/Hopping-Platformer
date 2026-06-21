using System.Collections.Generic;

public sealed class LevelData
{
    public readonly Dictionary<HexPos, PlatformNode> Nodes;

    public readonly HexPos Start;
    public readonly HexPos Goal;

    public LevelData(
        Dictionary<HexPos, PlatformNode> nodes,
        HexPos start,
        HexPos goal)
    {
        Nodes = nodes;

        Start = start;
        Goal = goal;
    }

    public List<HexPos> GetPathToGoal(
        HexPos start)
    {
        List<HexPos> path = new();

        if (!Nodes.TryGetValue(
            start,
            out PlatformNode node))
        {
            return path;
        }

        path.Add(start);

        while (node.CanReachGoal)
        {
            HexPos next =
                node.NextTowardsGoal;

            path.Add(next);

            node = Nodes[next];
        }

        return path;
    }
}
