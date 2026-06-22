using NUnit.Framework;

using HoppingPlatformer.Domain.Player;

namespace HoppingPlatformer.Tests.Domain.Player
{
    public class WalletTests
    {
        [Test]
        public void AddCoin_ShouldIncreaseCoinCount()
        {
            Wallet wallet = new Wallet();

            wallet.AddCoin();

            Assert.AreEqual(
                1,
                wallet.Coins);
        }
    }
}
