using NUnit.Framework;

using HoppingPlatformer.Application.Events;

namespace HoppingPlatformer.Tests.Application.Events
{
    public sealed class EventBusTests
    {
        [Test]
        public void Publish_ShouldInvokeListener()
        {
            EventBus bus =
                new EventBus();

            bool called = false;

            bus.Subscribe<PlayerDiedEvent>(
                _ => called = true);

            bus.Publish(
                new PlayerDiedEvent());

            Assert.IsTrue(called);
        }
    }
}