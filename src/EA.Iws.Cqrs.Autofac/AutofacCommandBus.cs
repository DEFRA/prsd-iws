namespace EA.Iws.Cqrs.Autofac
{
    using global::Autofac;
    using Core.Cqrs;
    using System.Threading.Tasks;

    public class AutofacCommandBus : ICommandBus
    {
        private readonly IComponentContext context;

        public AutofacCommandBus(IComponentContext context)
        {
            this.context = context;
        }

        public async Task SendAsync<T>(T command) where T : ICommand
        {
            ICommandHandler<T> commandHandler;

            if (context.TryResolve(out commandHandler))
            {
                await commandHandler.HandleAsync(command);
            }
            else
            {
                throw new MissingHandlerException("Handler missing for command type: " + typeof(T).Name +
                                                  ". Did you forget to create a command handler?");
            }
        }
    }
}