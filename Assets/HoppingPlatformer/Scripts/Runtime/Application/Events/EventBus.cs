using System;
using System.Collections.Generic;

namespace HoppingPlatformer.Application.Events
{
    public sealed class EventBus : IEventBus
    {
        private readonly Dictionary<Type, Delegate> _listeners = new();

        public void Publish<T>(T gameEvent) where T : IGameEvent
        {
            Type type = typeof(T);

            if (!_listeners.TryGetValue(type, out Delegate callback))
            {
                return;
            }

            Action<T> action = callback as Action<T>;

            action?.Invoke(gameEvent);
        }

        public void Subscribe<T>(Action<T> listener) where T : IGameEvent
        {
            Type type = typeof(T);

            if (_listeners.TryGetValue(type, out Delegate existing))
            {
                _listeners[type] = Delegate.Combine(existing, listener);
            }
            else
            {
                _listeners[type] = listener;
            }
        }

        public void Unsubscribe<T>(Action<T> listener) where T : IGameEvent
        {
            Type type = typeof(T);

            if (!_listeners.TryGetValue(type, out Delegate existing))
            {
                return;
            }

            Delegate updated = Delegate.Remove(existing, listener);

            if (updated == null)
            {
                _listeners.Remove(type);
            }
            else
            {
                _listeners[type] = updated;
            }
        }
    }
}