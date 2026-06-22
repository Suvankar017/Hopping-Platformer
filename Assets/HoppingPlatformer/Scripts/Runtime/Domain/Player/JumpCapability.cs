namespace HoppingPlatformer.Domain.Player
{
    public sealed class JumpCapability
    {
        public int Distance { get; private set; } = 1;

        public bool IsEnhanced { get; private set; }

        public void EnableDoubleJump()
        {
            Distance = 2;

            IsEnhanced = true;
        }

        public void Consume()
        {
            Distance = 1;

            IsEnhanced = false;
        }
    }
}