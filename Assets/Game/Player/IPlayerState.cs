namespace HoppingPlatformer.Player
{
    public interface IPlayerState
    {
        void Enter();

        void Exit();

        void Tick();

        void HandleJump(JumpDirection direction);
    }
}
