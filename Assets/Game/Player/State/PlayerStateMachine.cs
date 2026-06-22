namespace HoppingPlatformer.Player
{
    public sealed class PlayerStateMachine
    {
        private IPlayerState _current;

        public IPlayerState Current => _current;

        public void ChangeState(IPlayerState next)
        {
            _current?.Exit();

            _current = next;

            _current?.Enter();
        }

        public void Tick()
        {
            _current?.Tick();
        }

        public void HandleJump(JumpDirection direction)
        {
            _current?.HandleJump(direction);
        }
    }
}
