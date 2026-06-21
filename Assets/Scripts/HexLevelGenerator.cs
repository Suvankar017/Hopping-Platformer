using System.Collections.Generic;
using UnityEngine;

public sealed class HexLevelGenerator
{
    private readonly HexGridTopology _topology;

    public HexLevelGenerator(HexGridTopology topology)
    {
        _topology = topology;
    }

    //public LevelData Generate(
    //    int height,
    //    int startRow,
    //    int startColumn,
    //    LevelGenerationSettings settings)
    //{
    //    Dictionary<HexPos, PlatformNode> nodes = new();

    //    //----------------------------------
    //    // Start
    //    //----------------------------------

    //    //int startWidth = _topology.GetRowWidth(startRow);

    //    //HexPos start = new(startRow, Random.Range(0, startWidth));
    //    HexPos start = new(startRow, startColumn);

    //    nodes[start] = new PlatformNode(start, PlatformType.Start);

    //    //----------------------------------
    //    // Main Path
    //    //----------------------------------

    //    List<HexPos> mainPath = GenerateMainPath(start, height, nodes);

    //    HexPos goal = mainPath[^1];

    //    nodes[goal].Type = PlatformType.Goal;

    //    //return new LevelData(nodes, start, goal);

    //    //----------------------------------
    //    // Alternate Routes
    //    //----------------------------------

    //    GenerateAlternateBranches(mainPath, nodes, height, settings);

    //    //return new LevelData(nodes, start, goal);

    //    //----------------------------------
    //    // Reward Routes
    //    //----------------------------------

    //    GenerateRewardBranches(mainPath, nodes, height, settings);

    //    //return new LevelData(nodes, start, goal);

    //    EnsureSingleGoalPlatform(nodes, goal);

    //    return new LevelData(nodes, start, goal);
    //}

    public LevelData Generate(
    int height,
    int startRow,
    int startColumn,
    LevelGenerationSettings settings, out List<HexPos> originalMainPath)
    {
        Dictionary<HexPos, PlatformNode> nodes =
            new();

        //----------------------------------
        // Start
        //----------------------------------

        //int startWidth =
        //    _topology.GetRowWidth(startRow);

        //HexPos start =
        //    new(
        //        startRow,
        //        Random.Range(0, startWidth));

        HexPos start =
            new(
                startRow,
                startColumn);

        nodes[start] =
            new PlatformNode(
                start,
                PlatformType.Start);

        //----------------------------------
        // Main Path
        //----------------------------------

        List<HexPos> mainPath =
            GenerateMainPath(
                start,
                height,
                nodes);

        originalMainPath = mainPath;

        HexPos goal =
            mainPath[^1];

        nodes[goal].Type =
            PlatformType.Goal;

        //----------------------------------
        // Scaled Counts
        //----------------------------------

        int alternateBranches =
            settings.BaseAlternateBranches +
            Mathf.RoundToInt(
                height *
                settings.AlternateBranchesPerFloor);

        int rewardBranches =
            settings.BaseRewardBranches +
            Mathf.RoundToInt(
                height *
                settings.RewardBranchesPerFloor);

        //----------------------------------
        // Branches
        //----------------------------------

        GenerateAlternateBranches(
            mainPath,
            nodes,
            height,
            alternateBranches,
            settings);

        GenerateRewardBranches(
            mainPath,
            nodes,
            height,
            rewardBranches,
            settings);

        //----------------------------------
        // Cleanup
        //----------------------------------

        EnsureSingleGoalPlatform(
            nodes,
            goal);

        //MarkReachableNodes(
        //    nodes,
        //    goal);

        BuildGoalLinks(
            nodes,
            goal);

        GenerateItems(
            nodes,
            start,
            goal,
            settings);

        return new LevelData(
            nodes,
            start,
            goal);
    }

    private void BuildGoalLinks(
        Dictionary<HexPos, PlatformNode> nodes,
        HexPos goal)
    {
        Queue<HexPos> queue = new();

        queue.Enqueue(goal);

        while (queue.Count > 0)
        {
            HexPos current = queue.Dequeue();

            foreach (HexPos parent in _topology.GetDownNeighbours(current))
            {
                if (!nodes.TryGetValue(
                    parent,
                    out PlatformNode parentNode))
                {
                    continue;
                }

                if (parentNode.CanReachGoal)
                    continue;

                parentNode.CanReachGoal = true;
                parentNode.NextTowardsGoal = current;

                queue.Enqueue(parent);
            }
        }
    }

    private void MarkReachableNodes(
        Dictionary<HexPos, PlatformNode> nodes,
        HexPos goal)
    {
        Queue<HexPos> queue = new();

        queue.Enqueue(goal);

        nodes[goal].CanReachGoal = true;

        while (queue.Count > 0)
        {
            HexPos current =
                queue.Dequeue();

            foreach (HexPos parent
                in _topology.GetDownNeighbours(
                    current))
            {
                if (!nodes.TryGetValue(
                    parent,
                    out PlatformNode node))
                {
                    continue;
                }

                if (node.CanReachGoal)
                    continue;

                node.CanReachGoal = true;

                queue.Enqueue(parent);
            }
        }
    }

    private void GenerateItems(
        Dictionary<HexPos, PlatformNode> nodes,
        HexPos start,
        HexPos goal,
        LevelGenerationSettings settings)
    {
        foreach (PlatformNode node in nodes.Values)
        {
            if (node.Position.Equals(start))
                continue;

            if (node.Position.Equals(goal))
                continue;

            switch (node.Type)
            {
                case PlatformType.Reward:

                    node.Item =
                        Random.value < settings.GemChance
                            ? ItemType.Gem
                            : ItemType.DoubleJump;

                    break;

                case PlatformType.Normal:

                    if (Random.value < settings.CoinChance)
                    {
                        node.Item =
                            ItemType.Coin;
                    }

                    break;
            }
        }
    }

    private List<HexPos> GenerateMainPath(
        HexPos start,
        int height,
        Dictionary<HexPos, PlatformNode> nodes)
    {
        List<HexPos> path = new();

        HexPos current = start;

        path.Add(current);

        while (current.Row < height - 1)
        {
            List<HexPos> options = _topology.GetUpNeighbours(current);

            current = options[Random.Range(0, options.Count)];

            path.Add(current);

            if (!nodes.ContainsKey(current))
            {
                nodes[current] = new PlatformNode(current);
            }
        }

        return path;
    }

    //private void GenerateAlternateBranches(
    //    List<HexPos> mainPath,
    //    Dictionary<HexPos, PlatformNode> nodes,
    //    int height,
    //    LevelGenerationSettings settings)
    //{
    //    int count = settings.AlternateBranches;

    //    for (int i = 0; i < count; i++)
    //    {
    //        int startIndex = Random.Range(0, mainPath.Count - 3);

    //        HexPos current = mainPath[startIndex];

    //        int branchLength = Random.Range(
    //            settings.MinBranchLength,
    //            settings.MaxBranchLength + 1);

    //        for (int step = 0; step < branchLength; step++)
    //        {
    //            if (current.Row >= height - 2)
    //                break;

    //            float t = current.Row / (float)(height - 1);

    //            float density = settings.DensityCurve.Evaluate(t);

    //            if (Random.value > density)
    //                break;

    //            List<HexPos> options = _topology.GetUpNeighbours(current);

    //            if (options.Count == 0)
    //                break;

    //            HexPos next = options[Random.Range(0, options.Count)];

    //            if (!nodes.ContainsKey(next))
    //            {
    //                nodes[next] = new PlatformNode(next);
    //            }

    //            current = next;
    //        }
    //    }
    //}

    private void GenerateAlternateBranches(
    List<HexPos> mainPath,
    Dictionary<HexPos, PlatformNode> nodes,
    int height,
    int branchCount,
    LevelGenerationSettings settings)
    {
        int extraLength =
            Mathf.FloorToInt(
                height *
                settings.BranchLengthScale);

        int maxLength =
            settings.MaxBranchLength +
            extraLength;

        for (int i = 0; i < branchCount; i++)
        {
            int startIndex =
                Random.Range(
                    0,
                    mainPath.Count - 3);

            HexPos current =
                mainPath[startIndex];

            int branchLength =
                Random.Range(
                    settings.MinBranchLength,
                    maxLength + 1);

            for (int step = 0;
                 step < branchLength;
                 step++)
            {
                if (current.Row >= height - 2)
                    break;

                float t =
                    current.Row /
                    (float)(height - 1);

                float density =
                    settings.DensityCurve.Evaluate(t);

                if (Random.value > density)
                    break;

                List<HexPos> options =
                    _topology.GetUpNeighbours(current);

                if (options.Count == 0)
                    break;

                HexPos next =
                    options[
                        Random.Range(
                            0,
                            options.Count)];

                if (!nodes.ContainsKey(next))
                {
                    nodes[next] =
                        new PlatformNode(next);
                }

                current = next;
            }
        }
    }

    //private void GenerateRewardBranches(
    //    List<HexPos> mainPath,
    //    Dictionary<HexPos, PlatformNode> nodes,
    //    int height,
    //    LevelGenerationSettings settings)
    //{
    //    for (int i = 0; i < settings.RewardBranches; i++)
    //    {
    //        int startIndex = Random.Range(
    //                0,
    //                mainPath.Count - 4);

    //        HexPos current =
    //            mainPath[startIndex];

    //        int length =
    //            Random.Range(2, 5);

    //        HexPos last = current;

    //        for (int step = 0;
    //             step < length;
    //             step++)
    //        {
    //            if (current.Row >= height - 2)
    //                break;

    //            List<HexPos> options =
    //                _topology.GetUpNeighbours(
    //                    current);

    //            if (options.Count == 0)
    //                break;

    //            current =
    //                options[
    //                    Random.Range(
    //                        0,
    //                        options.Count)];

    //            if (!nodes.ContainsKey(current))
    //            {
    //                nodes[current] =
    //                    new PlatformNode(current);
    //            }

    //            last = current;
    //        }

    //        nodes[last].Type =
    //            PlatformType.Reward;
    //    }
    //}

    private void GenerateRewardBranches(
    List<HexPos> mainPath,
    Dictionary<HexPos, PlatformNode> nodes,
    int height,
    int rewardCount,
    LevelGenerationSettings settings)
    {
        int extraLength =
            Mathf.FloorToInt(
                height *
                settings.BranchLengthScale);

        int maxLength =
            settings.MaxBranchLength +
            extraLength;

        for (int i = 0;
             i < rewardCount;
             i++)
        {
            int startIndex =
                Random.Range(
                    0,
                    mainPath.Count - 4);

            HexPos current =
                mainPath[startIndex];

            HexPos last =
                current;

            int length =
                Random.Range(
                    settings.MinBranchLength,
                    maxLength + 1);

            for (int step = 0;
                 step < length;
                 step++)
            {
                if (current.Row >= height - 2)
                    break;

                List<HexPos> options =
                    _topology.GetUpNeighbours(
                        current);

                if (options.Count == 0)
                    break;

                current =
                    options[
                        Random.Range(
                            0,
                            options.Count)];

                if (!nodes.ContainsKey(current))
                {
                    nodes[current] =
                        new PlatformNode(current);
                }

                last = current;
            }

            if (nodes.TryGetValue(
                last,
                out PlatformNode node))
            {
                if (node.Type ==
                    PlatformType.Normal)
                {
                    node.Type =
                        PlatformType.Reward;
                }
            }
        }
    }

    private void EnsureSingleGoalPlatform(
        Dictionary<HexPos, PlatformNode> nodes,
        HexPos goal)
    {
        List<HexPos> remove = new();

        foreach (var pair in nodes)
        {
            if (pair.Key.Row != goal.Row)
                continue;

            if (pair.Key.Equals(goal))
                continue;

            remove.Add(pair.Key);
        }

        foreach (var pos in remove)
        {
            nodes.Remove(pos);
        }
    }
}
