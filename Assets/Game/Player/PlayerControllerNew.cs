using UnityEngine;

namespace HoppingPlatformer.Player
{
    public sealed class PlayerControllerNew : MonoBehaviour
    {
        [SerializeField]
        private PlayerMovementSettings _movementSettings = new();

        [SerializeField]
        private KeyboardPlayerInput _keyboardInput;

        [SerializeField]
        private MousePlayerInput _mouseInput;

        [SerializeField]
        private float _inputBufferDuration = 0.1f;

        private IPlayerMover _mover;
        private IPlayerInput _input;
        private PlayerStateMachine _stateMachine;
        private InputBuffer<JumpDirection> _inputBuffer;
        private IJumpService _jumpService;
        private IInteractionResolver _interactionResolver;

        private PlatformView _currentPlatform;
        private PlayerAbilities _abilities;

        //private IdleState _idleState;
        //private JumpingState _jumpingState;
        //private FallingState _fallingState;
        //private VictoryState _victoryState;
        //private DeadState _deadState;

        private void Awake()
        {
            _mover = new PlayerMover(this, _movementSettings);

            _input = new CompositePlayerInput(_keyboardInput, _mouseInput);

            _stateMachine = new PlayerStateMachine();

            _inputBuffer = new InputBuffer<JumpDirection>(_inputBufferDuration);

            _interactionResolver = new PlatformInteractionResolver();

            if (_mouseInput != null)
                _mouseInput.Init(transform);

            //_idleState = new IdleState(this);
            //_jumpingState = new JumpingState(this);
            //_fallingState = new FallingState(this);
            //_victoryState = new VictoryState(this);
            //_deadState = new DeadState(this);
        }

        private void OnEnable()
        {
            _input.JumpRequested += HandleJumpRequested;
        }

        private void OnDisable()
        {
            _input.JumpRequested -= HandleJumpRequested;
        }

        private void Update()
        {
            _stateMachine.Tick();
        }

        private void HandleJumpRequested(JumpDirection direction)
        {
            Debug.Log($"Requested Jump Direction: {direction}");
            //_stateMachine.HandleJump(direction);
        }

        private void BufferInput(JumpDirection direction)
        {
            _inputBuffer.Store(direction, Time.time);

            TryConsumeBufferedInput();
        }

        private void TryConsumeBufferedInput()
        {
            if (!_inputBuffer.TryConsume(Time.time, out JumpDirection direction))
            {
                return;
            }

            ExecuteJump(direction);
        }

        public void Init(LevelRuntime level)
        {
            EnterIdle();

            _jumpService = new HexJumpService(level);
        }

        public bool TryConsumeBufferedJump(out JumpDirection direction)
        {
            return _inputBuffer.TryConsume(Time.time, out direction);
        }

        private void ExecuteJump(JumpDirection direction)
        {
            JumpResult result = _jumpService.ResolveJump(_currentPlatform, direction, _abilities.JumpDistance);

            if (!result.Success)
            {
                StartFall(result.FailedPosition);
                return;
            }

            StartJump(result.Platform);
        }

        private void StartJump(PlatformView target)
        {
            EnterJumping();

            Vector3 platformOffset = new(0.0f, 0.125f);

            Vector3 targetPosition = target.transform.position + platformOffset;

            _mover.JumpTo(transform, targetPosition, () => Land(target));
        }

        private void StartFall(HexPos targetPosition)
        {

        }

        private void Land(PlatformView target)
        {

        }

        private void ResolveInteraction(PlatformView platform)
        {
            _interactionResolver.Resolve(platform, this);
        }

        public void RaiseDeadEvent()
        {

        }

        public void RaiseVictoryEvent()
        {

        }

        public void EnterIdle()
        {
            _stateMachine.ChangeState(new IdleState(this));
        }

        public void EnterJumping()
        {
            _stateMachine.ChangeState(new JumpingState(this));
        }

        public void EnterFalling()
        {
            _stateMachine.ChangeState(new FallingState(this));
        }

        public void EnterVictory()
        {
            _stateMachine.ChangeState(new VictoryState(this));
        }

        public void EnterDead()
        {
            _stateMachine.ChangeState(new DeadState(this));
        }
    }
}
