namespace HoppingPlatformer.Player
{
    public sealed class IdleState : PlayerStateBase
    {
        public IdleState(PlayerControllerNew player) : base(player)
        {
        }

        public override void HandleJump(JumpDirection direction)
        {
            //Player.ExecuteJump(direction);
        }
    }
}
