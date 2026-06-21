using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float jumpDuration = 0.2f;

    [SerializeField]
    private float jumpHeight = 0.75f;

    [SerializeField]
    private float fallSpeed = 10f;

    [SerializeField]
    private float inputBufferDuration = 0.5f;

    [SerializeField]
    private bool autoTraverse = false;

    private LevelRuntime _level;

    private PlatformView _currentPlatform;

    private PlayerState _state;

    private readonly PlayerAbilities _abilities = new();

    private bool _hasBufferedInput;

    private JumpDirection _bufferedDirection;

    private float _bufferTime;

    private Camera _mainCamera;

    private Vector2 _hexSize;

    private int _height;

    private int _coins;

    public LevelRuntime Level => _level;

    public event System.Action OnVictory;
    public event System.Action OnDead;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    public void AddCoin()
    {
        _coins++;

        Debug.Log($"Coins: {_coins}");
    }

    private List<HexPos> _autoPath;

    public void Initialize(LevelRuntime level, Vector2 hexSize, int height)
    {
        _level = level;

        _hexSize = hexSize;

        _height = height;

        PlatformView start =
            level.Platforms[level.Start];

        _currentPlatform = start;
        
        Vector3 platformOffset = new Vector3(0.0f, 0.125f);

        transform.position =
            start.transform.position + platformOffset;

        _state = PlayerState.Idle;

        //var mainPath = _level.MainPath;
        //mainPath.Sort((a, b) => a.Row.CompareTo(b.Row));
        
        if (autoTraverse)
        {
            currentIndex = 0;
            _autoPath = _level.GetPathToGoal(_currentPlatform.Node.Position);
            Invoke(nameof(AutoTraverse), 1.0f);

            Color pathColor = Random.ColorHSV();
            foreach (var path in _autoPath)
            {
                if (!_level.Platforms.TryGetValue(path, out PlatformView view))
                    continue;

                view.GetComponentInChildren<SpriteRenderer>().color = pathColor;
            }
        }
    }

    private void AutoTraverse()
    {
        var path = _autoPath;

        //if (currentIndex < 0)
        //    currentIndex = 0;

        HexPos current = path[currentIndex];
        HexPos target = path[currentIndex + 1];

        currentIndex++;

        int currentWidth = _level.Topology.GetRowWidth(current.Row);

        int nextWidth = _level.Topology.GetRowWidth(target.Row);

        bool currentLong = currentWidth > nextWidth;

        if (currentLong)
        {
            int diff = target.Col - current.Col;
            BufferInput((diff >= 0) ? JumpDirection.Right : JumpDirection.Left);
        }
        else
        {
            int diff = target.Col - current.Col;
            BufferInput((diff > 0) ? JumpDirection.Right : JumpDirection.Left);
        }
    }

    private int currentIndex;

    private void Update()
    {
        if (_state == PlayerState.Dead)
            return;

        if (_state == PlayerState.Victory)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouse =
                _mainCamera.ScreenToWorldPoint(
                    Input.mousePosition);

            JumpDirection direction =
                mouse.x < transform.position.x
                    ? JumpDirection.Left
                    : JumpDirection.Right;

            BufferInput(direction);
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            JumpDirection direction = JumpDirection.Left;

            BufferInput(direction);
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            JumpDirection direction = JumpDirection.Right;

            BufferInput(direction);
        }
    }

    private void BufferInput(
    JumpDirection direction)
    {
        _hasBufferedInput = true;

        _bufferedDirection = direction;

        _bufferTime = Time.time;

        if (_state == PlayerState.Idle)
        {
            ConsumeBufferedInput();
        }
    }

    private void ConsumeBufferedInput()
    {
        if (!_hasBufferedInput)
            return;

        if (Time.time - _bufferTime >
            inputBufferDuration)
        {
            _hasBufferedInput = false;
            return;
        }

        _hasBufferedInput = false;

        ExecuteJump(
            _bufferedDirection);
    }

    private void ExecuteJump(
    JumpDirection direction)
    {
        HexPos targetPosition;

        bool valid =
            direction == JumpDirection.Left
                ? _level.Topology.TryGetLeftJump(
                    _currentPlatform.Node.Position,
                    _abilities.JumpDistance,
                    out targetPosition)
                : _level.Topology.TryGetRightJump(
                    _currentPlatform.Node.Position,
                    _abilities.JumpDistance,
                    out targetPosition);

        if (!valid)
        {
            StartCoroutine(
                FallRoutine(targetPosition));

            return;
        }

        if (!_level.TryGetPlatform(
            targetPosition,
            out PlatformView target))
        {
            StartCoroutine(
                FallRoutine(targetPosition));

            return;
        }

        StartCoroutine(
            JumpRoutine(target));
    }

    private IEnumerator JumpRoutine(
        PlatformView target)
    {
        _state = PlayerState.Jumping;

        Vector3 platformOffset = new Vector3(0.0f, 0.125f);

        Vector3 start = transform.position;

        Vector3 end = target.transform.position + platformOffset;

        float elapsed = 0f;

        while (elapsed < jumpDuration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / jumpDuration;

            Vector3 pos = Vector3.Lerp(start, end, t);

            pos.y += Mathf.Sin(t * Mathf.PI) * jumpHeight;

            transform.position = pos;

            yield return null;
        }

        transform.position = end;

        Land(target);
    }

    private void Land(PlatformView platform)
    {
        _currentPlatform = platform;

        //ResolveInteraction(platform);

        if (_abilities.JumpDistance > 1)
        {
            _abilities.Consume();
        }

        if (platform.Node.Type == PlatformType.Goal)
        {
            _state = PlayerState.Victory;

            Debug.Log("WIN");

            OnVictory?.Invoke();

            return;
        }

        _state = PlayerState.Idle;

        ResolveInteraction(platform);

        ConsumeBufferedInput();

        if (autoTraverse)
            AutoTraverse();
    }

    private IEnumerator FallRoutine(HexPos targetPos)
    {
        _state = PlayerState.Falling;

        int width = _level.Topology.Width;
        bool evenWidth = (width & 1) == 0;

        int maxColumns = evenWidth
            ? Mathf.Max(width - 2, 0)
            : Mathf.Max(width - 1, 0);

        Vector3 centerOffset = new(
            maxColumns * Mathf.Sqrt(3f) * _hexSize.x,
            Mathf.Max(_height - 1, 0) * 1.5f * _hexSize.y,
            0f);

        centerOffset = Vector3.Scale(centerOffset, Vector3.right);

        Vector3 target = HexGridUtility.HexToWorld(
            targetPos.Col,
            targetPos.Row,
            _hexSize,
            evenWidth);

        target -= centerOffset * 0.5f;

        Vector3 platformOffset = new Vector3(0.0f, 0.125f);

        target += platformOffset;

        Vector3 start = transform.position;

        float elapsed = 0f;

        float speed = Vector3.Distance(start, target) / jumpDuration;

        while (elapsed < jumpDuration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / jumpDuration;

            Vector3 pos = Vector3.Lerp(start, target, t);

            pos.y +=
                Mathf.Sin(t * Mathf.PI)
                * jumpHeight;

            transform.position = pos;

            yield return null;
        }

        float deathY = target.y - 2.0f;

        while (transform.position.y > deathY)
        {
            transform.position +=
                Vector3.down *
                /*fallSpeed*/speed *
                Time.deltaTime;

            yield return null;
        }

        _state = PlayerState.Dead;

        Debug.Log("GAME OVER");

        OnDead?.Invoke();
    }

    public void GrantDoubleJump()
    {
        _abilities.GrantDoubleJump();
    }

    private void ResolveInteraction(
    PlatformView platform)
    {
        var interaction =
            platform.GetComponentInChildren<
                IPlatformInteraction>();

        interaction?.Interact(this);
    }

    public void PerformSkipJump(
        int skipFloors)
    {
        //if (!_level.TryGetSkipTarget(
        //    _currentPlatform,
        //    skipFloors,
        //    out PlatformView target))
        //{
        //    return;
        //}

        PlatformView target = null;

        for (int i = skipFloors; i >= 0; i--)
        {
            if (_level.TryGetSkipTarget(_currentPlatform, i, out target))
                break;
        }

        if (target == null)
            return;

        currentIndex = -1;
        _autoPath = _level.GetPathToGoal(target.Node.Position);

        Color pathColor = Random.ColorHSV();
        foreach (var path in _autoPath)
        {
            if (!_level.Platforms.TryGetValue(path, out PlatformView view))
                continue;

            view.GetComponentInChildren<SpriteRenderer>().color = pathColor;
        }

        StartCoroutine(JumpRoutine(target));
    }
}
