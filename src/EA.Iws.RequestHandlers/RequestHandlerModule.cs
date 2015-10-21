namespace EA.Iws.RequestHandlers
{
    using Authorization;
    using Autofac;
    using Copy;
    using Decorators;
    using Domain;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Domain.NotificationConsent;
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
                typeof(RequestAuthorizationDecorator<,>),
                typeof(AuthenticationRequestHandlerDecorator<,>),
                typeof(NotificationReadOnlyAuthorizeDecorator<,>)); // <-- outer most decorator

            builder.RegisterAssemblyTypes()
                .AsClosedTypesOf(typeof(IRequest<>))
                .AsImplementedInterfaces();

            // Register the map classes
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Namespace != null && t.Namespace.Contains("Mappings"))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(ThisAssembly).AsClosedTypesOf(typeof(IEventHandler<>)).AsImplementedInterfaces();

            builder.RegisterType<NotificationToNotificationCopy>().AsSelf();
            builder.RegisterType<TransportRouteToTransportRouteCopy>().AsSelf();
            builder.RegisterType<WasteRecoveryToWasteRecoveryCopy>().AsSelf();
            builder.RegisterType<ExporterToExporterCopy>().AsSelf();

            builder.RegisterType<WasteCodeCopy>().AsSelf();
            builder.RegisterType<MovementFactory>().AsSelf();
            builder.RegisterType<NotificationMovementsQuantity>().AsSelf();
            builder.RegisterType<SetActualDateOfShipment>().AsSelf();
            builder.RegisterType<NotificationChargeCalculator>().AsSelf();
            builder.RegisterType<CertificateFactory>().AsSelf();
            builder.RegisterType<CertificateOfReceiptNameGenerator>().AsSelf();
            builder.RegisterType<CertificateOfRecoveryNameGenerator>().AsSelf();
            builder.RegisterType<ConsentNotification>().AsSelf();
            builder.RegisterType<MovementFileNameGenerator>().AsSelf();

            builder.RegisterType<NotificationNumberGenerator>().As<INotificationNumberGenerator>();
            builder.RegisterType<WorkingDayCalculator>().As<IWorkingDayCalculator>();
            builder.RegisterType<NotificationProgressService>().As<INotificationProgressService>();

            builder.RegisterType<AuthorizationManager>().As<IAuthorizationManager>();
            builder.RegisterType<UserRoleService>().As<IUserRoleService>();
            builder.RegisterType<RequestAuthorizationAttributeCache>().AsSelf().SingleInstance();
            builder.RegisterType<InMemoryAuthorizationService>().As<IAuthorizationService>();
        }
    }
}