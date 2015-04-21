namespace EA.Iws.Core.Cqrs
{
    using System.Threading.Tasks;
    using Domain;

    public class EventDispatcherCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
    {
        private readonly IDeferredEventDispatcher eventDispatcher;
        private readonly ICommandHandler<TCommand> inner;

        public EventDispatcherCommandHandlerDecorator(ICommandHandler<TCommand> inner,
            IDeferredEventDispatcher eventDispatcher)
        {
            this.inner = inner;
            this.eventDispatcher = eventDispatcher;
        }

        public async Task HandleAsync(TCommand command)
        {
            DomainEvents.Dispatcher = eventDispatcher;
            await inner.HandleAsync(command);
            await eventDispatcher.Resolve();
        }
    }
}