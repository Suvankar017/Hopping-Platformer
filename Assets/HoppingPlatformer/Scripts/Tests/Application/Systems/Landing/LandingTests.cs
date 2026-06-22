using NUnit.Framework;

using HoppingPlatformer.Domain.Common;
using HoppingPlatformer.Application.Events;
using HoppingPlatformer.Application.Systems.Landing;

namespace HoppingPlatformer.Tests.Application.Systems.Landing
{
    public class LandingTests
    {
        [Test]
        public void LandingSystem_ShouldPublishLandingEvent()
        {
            EventBus bus =
                new EventBus();

            bool landed = false;

            LandingSystem landing =
                new LandingSystem(bus);

            bus.Subscribe<PlayerLandedEvent>(
                _ => landed = true);

            bus.Publish(
                new PlayerMovedEvent(
                    new HexPosition(0, 0),
                    new HexPosition(1, 0)));

            Assert.IsTrue(landed);
        }
    }
}
