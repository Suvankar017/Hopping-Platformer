using HoppingPlatformer.Domain.Common;

namespace HoppingPlatformer.Domain.Player
{
    public sealed class Player
    {
        public HexPosition Position
        {
            get;
            private set;
        }

        public PlayerState State
        {
            get;
            private set;
        }

        public Wallet Wallet
        {
            get;
        }

        public JumpCapability Jump
        {
            get;
        }

        public Player(HexPosition startPosition)
        {
            Position = startPosition;

            State = PlayerState.Idle;

            Wallet = new Wallet();

            Jump = new JumpCapability();
        }

        public void MoveTo(HexPosition position)
        {
            Position = position;
        }

        public void ChangeState(PlayerState state)
        {
            State = state;
        }
    }
}
