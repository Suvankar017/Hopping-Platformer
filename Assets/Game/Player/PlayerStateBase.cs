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

        public virtual void HandleJump(JumpDirection direction)
        {
        }
    }

    public sealed class IdleState : PlayerStateBase
    {
        public IdleState(PlayerControllerNew player) : base(player)
        {
        }

        public override void HandleJump(JumpDirection direction)
        {
            Player.ExecuteJump(direction);
        }
    }

    public sealed class JumpingState : PlayerStateBase
    {
        public JumpingState(PlayerControllerNew player) : base(player)
        {
        }
    }

    public sealed class FallingState : PlayerStateBase
    {
        public FallingState(PlayerControllerNew player) : base(player)
        {
        }
    }

    public sealed class DeadState : PlayerStateBase
    {
        public DeadState(PlayerControllerNew player) : base(player)
        {
        }

        public override void Enter()
        {
            Player.RaiseDeadEvent();
        }
    }

    public sealed class VictoryState : PlayerStateBase
    {
        public VictoryState(PlayerControllerNew player) : base(player)
        {
        }

        public override void Enter()
        {
            Player.RaiseVictoryEvent();
        }
    }
}
