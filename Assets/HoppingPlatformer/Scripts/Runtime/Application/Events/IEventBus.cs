using System;

namespace HoppingPlatformer.Application.Events
{
    public interface IEventBus
    {
        void Publish<T>(T gameEvent) where T : IGameEvent;

        void Subscribe<T>(Action<T> listener) where T : IGameEvent;

        void Unsubscribe<T>(Action<T> listener) where T : IGameEvent;
    }
}