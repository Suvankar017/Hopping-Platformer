namespace HoppingPlatformer.Player
{
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
