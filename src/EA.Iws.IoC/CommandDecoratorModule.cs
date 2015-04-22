namespace EA.Iws.IoC
{
    using Autofac;
    using Core.Cqrs;

    public class CommandDecoratorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var cqrsAssembly = typeof(ICommandBus).Assembly;

            // Command Handlers
            builder.RegisterAssemblyTypes(cqrsAssembly)
                .AsNamedClosedTypesOf(typeof(ICommandHandler<>), t => "command_handler");

            // Order matters here
            builder.RegisterGenericDecorators(cqrsAssembly, typeof(ICommandHandler<>), "command_handler",
                typeof(TransactionCommandHandlerDecorator<>), // <-- inner most decorator
                typeof(EventDispatcherCommandHandlerDecorator<>),
                typeof(ValidationCommandHandlerDecorator<>),
                typeof(AuthorizationCommandHandlerDecorator<>)); // <-- outer most decorator
        }
    }
}