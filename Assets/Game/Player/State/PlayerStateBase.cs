namespace HoppingPlatformer.Player
{
    public abstract class PlayerStateBase : IPlayerState
    {
        protected readonly PlayerControllerNew Player;

        protected PlayerStateBase(PlayerControllerNew player)
        {
            Player = player;
        }

        public virtual void Enter() { }

        public virtual void Exit() { }

        public virtual void Tick() { }

        public virtual void HandleJump(JumpDirection direction) { }
    }
}
