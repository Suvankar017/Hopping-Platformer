namespace HoppingPlatformer.Player
{
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
}
