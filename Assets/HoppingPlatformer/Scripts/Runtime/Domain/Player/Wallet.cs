namespace HoppingPlatformer.Domain.Player
{
    public sealed class Wallet
    {
        public int Coins { get; private set; }

        public void AddCoin()
        {
            Coins++;
        }
    }
}
