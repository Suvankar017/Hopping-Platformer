using System;
using System.Collections.Generic;

namespace HoppingPlatformer.Application.Commands
{
    public sealed class CommandBus : ICommandBus
    {
        private readonly Dictionary<Type, object> _handlers = new();

        public void Register<T>(ICommandHandler<T> handler) where T : ICommand
        {
            _handlers[typeof(T)] = handler;
        }

        public void Execute<T>(T command) where T : ICommand
        {
            if (!_handlers.TryGetValue(typeof(T), out object handler))
            {
                throw new InvalidOperationException($"No handler registered for {typeof(T)}");
            }

            ICommandHandler<T> commandHandler = handler as ICommandHandler<T>;

            commandHandler.Handle(command);
        }
    }
}