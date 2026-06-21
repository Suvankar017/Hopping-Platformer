using System.Collections.Generic;
using UnityEngine;

public sealed class HexLevelSpawner : MonoBehaviour
{
    [SerializeField] private GameObject platformPrefab;

    [SerializeField] private int width = 3;
    [SerializeField] private int height = 11;

    [SerializeField] private int startRow = 0;
    [SerializeField] private int startColumn = 0;

    //[SerializeField] private float branchChance = 0.35f;

    [SerializeField] private Vector2 hexSize = Vector2.one;

    [SerializeField]
    private LevelGenerationSettings generationSettings = new();

    [SerializeField] private int testCount = 1000;

    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private CameraController cameraController;

    [SerializeField]
    private GameObject coinPrefab;
    [SerializeField]
    private Vector3 coinSpawnOffset;

    [SerializeField]
    private GameObject gemPrefab;
    [SerializeField]
    private Vector3 gemSpawnOffset;

    [SerializeField]
    private GameObject doubleJumpPrefab;
    [SerializeField]
    private Vector3 doubleJumpSpawnOffset;

    private void OnEnable()
    {
        playerController.OnVictory += OnPlayerGotVictory;
        playerController.OnDead += OnPlayerIsDead;
    }

    private void OnDisable()
    {
        playerController.OnVictory -= OnPlayerGotVictory;
        playerController.OnDead -= OnPlayerIsDead;
    }

    private void OnPlayerGotVictory()
    {
        height += 10;
        Start();
    }

    private void OnPlayerIsDead()
    {
        LevelRuntime level = playerController.Level;

        playerController.Initialize(level, hexSize, height);

        cameraController.Initialize(playerController.transform);
    }

    private void Start()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
            Destroy(transform.GetChild(i).gameObject);

        LevelRuntime level = Generate();

        playerController.Initialize(level, hexSize, height);

        cameraController.Initialize(playerController.transform);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
                Destroy(transform.GetChild(i).gameObject);

            LevelRuntime level = Generate();

            playerController.Initialize(level, hexSize, height);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Test();
        }
    }

    private void Test()
    {
        var topology =
            new HexGridTopology(width);

        var generator =
            new HexLevelGenerator(topology);

        //LevelGenerationSettings settings = new()
        //{
        //    //AlternateBranches = 3,
        //    //RewardBranches = 2,
        //    //MinBranchLength = 2,
        //    //MaxBranchLength = 4

        //    BaseAlternateBranches = 2,
        //    AlternateBranchesPerFloor = 0.15f,

        //    BaseRewardBranches = 1,
        //    RewardBranchesPerFloor = 0.05f
        //};

        int wrongGoalCount = 0;

        for (int i = 0; i < testCount; i++)
        {
            LevelData level = generator.Generate(
                height,
                startRow,
                startColumn,
                generationSettings,
                out List<HexPos> mainPath);

            HexPos goal = level.Goal;

            foreach (var pair in level.Nodes)
            {
                if (pair.Key.Row != goal.Row)
                    continue;

                if (pair.Key.Equals(goal))
                    continue;

                wrongGoalCount++;
            }
        }

        Debug.Log($"Wrong Goal Count: {wrongGoalCount}");
    }

    public LevelRuntime Generate()
    {
        bool evenWidth = (width & 1) == 0;

        int maxColumns = evenWidth
            ? Mathf.Max(width - 2, 0)
            : Mathf.Max(width - 1, 0);

        Vector3 centerOffset = new(
            maxColumns * Mathf.Sqrt(3f) * hexSize.x,
            Mathf.Max(height - 1, 0) * 1.5f * hexSize.y,
            0f);

        centerOffset = Vector3.Scale(centerOffset, Vector3.right);

        var topology =
            new HexGridTopology(width);

        var generator =
            new HexLevelGenerator(topology);

        //LevelData level =
        //    generator.Generate(
        //        height,
        //        startRow,
        //        startColumn,
        //        branchChance);

        //LevelGenerationSettings settings = new()
        //{
        //    AlternateBranches = 3,
        //    RewardBranches = 2,
        //    MinBranchLength = 2,
        //    MaxBranchLength = 4
        //};

        LevelData level =
            generator.Generate(
                height,
                startRow,
                startColumn,
                generationSettings,
                out List<HexPos> mainPath);

        //foreach (HexPos pos in level.Platforms)
        //{
        //    Vector3 world =
        //        HexGridUtility.HexToWorld(
        //            pos.Col,
        //            pos.Row,
        //            hexSize,
        //            evenWidth);

        //    Instantiate(
        //        platformPrefab,
        //        world - centerOffset * 0.5f,
        //        Quaternion.identity,
        //        transform);
        //}

        Dictionary<HexPos, PlatformView> views = new();

        foreach (PlatformNode node in level.Nodes.Values)
        {
            HexPos pos = node.Position;

            Vector3 world =
                HexGridUtility.HexToWorld(
                    pos.Col,
                    pos.Row,
                    hexSize,
                    evenWidth);

            GameObject platform = Instantiate(
                platformPrefab,
                world - centerOffset * 0.5f,
                Quaternion.identity,
                transform);

            switch (node.Type)
            {
                case PlatformType.Normal:
                    platform.GetComponentInChildren<SpriteRenderer>().color = Color.grey;
                    break;
                case PlatformType.Start:
                    platform.GetComponentInChildren<SpriteRenderer>().color = Color.blue;
                    break;
                case PlatformType.Goal:
                    platform.GetComponentInChildren<SpriteRenderer>().color = Color.green;
                    break;
                case PlatformType.Reward:
                    platform.GetComponentInChildren<SpriteRenderer>().color = Color.magenta;
                    break;
            }

            PlatformView view = platform.AddComponent<PlatformView>();

            view.Initialize(node);

            views.Add(
                node.Position,
                view);

            SpawnItem(
                node,
                platform.transform);
        }

        return new LevelRuntime(
            views,
            topology,
            level.Start,
            level.Goal,
            mainPath,
            level);
    }

    private void SpawnItem(
        PlatformNode node,
        Transform parent)
    {
        GameObject prefab = null;
        Vector3 spawnOffset = Vector3.zero;

        switch (node.Item)
        {
            case ItemType.Coin:
                prefab = coinPrefab;
                spawnOffset = coinSpawnOffset;
                break;

            case ItemType.Gem:
                prefab = gemPrefab;
                spawnOffset = gemSpawnOffset;
                break;

            case ItemType.DoubleJump:
                prefab = doubleJumpPrefab;
                spawnOffset = doubleJumpSpawnOffset;
                break;
        }

        if (prefab == null)
            return;

        Instantiate(
            prefab,
            parent.position + spawnOffset,
            Quaternion.identity,
            parent);
    }
}