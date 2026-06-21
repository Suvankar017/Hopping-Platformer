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

        private void Awake()
        {
            _mover = new PlayerMover(this, _movementSettings);

            _input = new CompositePlayerInput(_keyboardInput, _mouseInput);

            _stateMachine = new PlayerStateMachine();

            _inputBuffer = new InputBuffer<JumpDirection>(_inputBufferDuration);

            if (_mouseInput != null)
                _mouseInput.Init(transform);
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
            _stateMachine.HandleJump(direction);
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

        public void Init()
        {
            EnterIdle();
        }

        public bool TryConsumeBufferedJump(out JumpDirection direction)
        {
            return _inputBuffer.TryConsume(Time.time, out direction);
        }

        public void ExecuteJump(JumpDirection direction)
        {

        }

        public void RaiseDeadEvent()
        {

        }

        public void RaiseVictoryEvent()
        {

        }

        public void EnterFalling()
        {
            _stateMachine.ChangeState(new FallingState(this));
        }

        public void EnterVictory()
        {
            _stateMachine.ChangeState(new VictoryState(this));
        }

        public void EnterIdle()
        {
            _stateMachine.ChangeState(new IdleState(this));
        }
    }
}
