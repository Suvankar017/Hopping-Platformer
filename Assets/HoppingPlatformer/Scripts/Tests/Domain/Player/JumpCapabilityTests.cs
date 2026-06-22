using NUnit.Framework;

using HoppingPlatformer.Domain.Player;

namespace HoppingPlatformer.Tests.Domain.Player
{
    public class JumpCapabilityTests
    {
        [Test]
        public void EnableDoubleJump_ShouldSetDistanceToTwo()
        {
            JumpCapability jump =
                new JumpCapability();

            jump.EnableDoubleJump();

            Assert.AreEqual(
                2,
                jump.Distance);

            Assert.IsTrue(
                jump.IsEnhanced);
        }

        [Test]
        public void Consume_ShouldResetDistance()
        {
            JumpCapability jump =
                new JumpCapability();

            jump.EnableDoubleJump();

            jump.Consume();

            Assert.AreEqual(
                1,
                jump.Distance);

            Assert.IsFalse(
                jump.IsEnhanced);
        }
    }
}
