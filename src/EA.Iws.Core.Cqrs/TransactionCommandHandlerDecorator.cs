namespace EA.Iws.Core.Cqrs
{
    using System.Threading.Tasks;
    using System.Transactions;

    public class TransactionCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> inner;

        public TransactionCommandHandlerDecorator(ICommandHandler<TCommand> inner)
        {
            this.inner = inner;
        }

        public async Task HandleAsync(TCommand command)
        {
            using (var scope = new TransactionScope())
            {
                await inner.HandleAsync(command);

                scope.Complete();
            }
        }
    }
}