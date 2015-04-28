namespace EA.Iws.Cqrs.Autofac
{
    using global::Autofac;
    using Core.Cqrs;
    using Registration;

    public class CommandModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // We want to dependency inject our CQRS class library.
            var cqrsAssembly = typeof(CreateOrganisation).Assembly;

            // Register all ICommand types.
            builder.RegisterAssemblyTypes(cqrsAssembly)
                .Where(t => typeof(ICommand).IsAssignableFrom(t))
                .As<ICommand>();

            // Register our implementation of the command bus.
            builder.RegisterType<AutofacCommandBus>().As<ICommandBus>();
        }
    }
}