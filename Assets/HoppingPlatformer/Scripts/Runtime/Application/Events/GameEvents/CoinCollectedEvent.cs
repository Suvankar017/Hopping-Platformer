namespace HoppingPlatformer.Application.Events
{
    public readonly struct CoinCollectedEvent : IGameEvent
    {
        public int TotalCoins { get; }

        public CoinCollectedEvent(int totalCoins)
        {
            TotalCoins = totalCoins;
        }
    }
}