namespace EA.Iws.Cqrs.Autofac
{
    using global::Autofac;
    using Core.Cqrs;
    using Registration;

    public class QueryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var cqrsAssembly = typeof(CreateOrganisation).Assembly;

            // Register all IQuery<T> types.
            builder.RegisterAssemblyTypes(cqrsAssembly)
                .AsClosedTypesOf(typeof(IQuery<>))
                .AsImplementedInterfaces();

            // Register all IQueryHandler<T>.
            builder.RegisterAssemblyTypes(cqrsAssembly)
                .AsClosedTypesOf(typeof(IQueryHandler<,>))
                .AsImplementedInterfaces();

            // Register our implementation of the command bus.
            builder.RegisterType<AutofacQueryBus>().As<IQueryBus>();
        }
    }
}
