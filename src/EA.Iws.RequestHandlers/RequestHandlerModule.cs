namespace EA.Iws.RequestHandlers
{
    using Autofac;
    using Copy;
    using Domain;
    using Domain.NotificationApplication;
    using Movement;
    using Notification;
    using Prsd.Core.Autofac;
    using Prsd.Core.Decorators;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;

    public class RequestHandlerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .AsNamedClosedTypesOf(typeof(IRequestHandler<,>), t => "request_handler");

            // Order matters here
            builder.RegisterGenericDecorators(ThisAssembly, typeof(IRequestHandler<,>), "request_handler",
                typeof(EventDispatcherRequestHandlerDecorator<,>), // <-- inner most decorator
                typeof(AuthorizationRequestHandlerDecorator<,>),
                typeof(AuthenticationRequestHandlerDecorator<,>)); // <-- outer most decorator

            builder.RegisterAssemblyTypes()
                .AsClosedTypesOf(typeof(IRequest<>))
                .AsImplementedInterfaces();

            // Register the map classes
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Namespace.Contains("Mappings"))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(ThisAssembly).AsClosedTypesOf(typeof(IEventHandler<>)).AsImplementedInterfaces();

            builder.RegisterType<NotificationToNotificationCopy>().AsSelf();

            builder.RegisterType<NotificationNumberGenerator>().As<INotificationNumberGenerator>();
            builder.RegisterType<NotificationChargeCalculator>().As<INotificationChargeCalculator>();
            builder.RegisterType<WorkingDayCalculator>().As<IWorkingDayCalculator>();
            builder.RegisterType<NotificationProgressService>().As<INotificationProgressService>();
            builder.RegisterType<NotificationMovementService>().As<INotificationMovementService>();
        }
    }
}