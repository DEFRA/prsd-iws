namespace EA.Iws.RequestHandlers
{
    using System;
    using System.IO;
    using Aspose.Words;
    using Authorization;
    using Autofac;
    using Copy;
    using Decorators;
    using Documents;
    using Domain;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using Domain.NotificationConsent;
    using ImportNotification;
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

            builder.RegisterGeneric(typeof(GetDraftDataHandler<>))
                .Named("request_handler", typeof(IRequestHandler<,>));

            builder.RegisterGeneric(typeof(SetDraftDataHandler<>))
                .Named("request_handler", typeof(IRequestHandler<,>));

            // Order matters here
            builder.RegisterGenericDecorators(ThisAssembly, typeof(IRequestHandler<,>), "request_handler",
                typeof(EventDispatcherRequestHandlerDecorator<,>), // <-- inner most decorator
                typeof(RequestAuthorizationDecorator<,>),
                typeof(AuthenticationRequestHandlerDecorator<,>),
                typeof(NotificationReadOnlyAuthorizeDecorator<,>)); // <-- outer most decorator

            // Register the map classes
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Namespace != null && t.Namespace.Contains("Mappings"))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .AsClosedTypesOf(typeof(IEventHandler<>))
                .AsImplementedInterfaces();

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
            builder.RegisterType<DecisionRequiredBy>().AsSelf();

            builder.RegisterType<NotificationNumberGenerator>().As<INotificationNumberGenerator>();
            builder.RegisterType<WorkingDayCalculator>().As<IWorkingDayCalculator>();
            builder.RegisterType<NotificationProgressService>().As<INotificationProgressService>();

            if (HasAsposeLicense())
            {
                builder.RegisterType<AsposePdfGenerator>().As<IPdfGenerator>();
            }

            builder.RegisterType<AuthorizationManager>().As<IAuthorizationManager>();
            builder.RegisterType<UserRoleService>().As<IUserRoleService>();
            builder.RegisterType<RequestAuthorizationAttributeCache>().AsSelf().SingleInstance();
            builder.RegisterType<InMemoryAuthorizationService>().As<IAuthorizationService>();
        }

        private static bool HasAsposeLicense()
        {
            var license = Path.Combine((string)AppDomain.CurrentDomain.GetData("DataDirectory"), "Aspose.Words.lic");

            if (File.Exists(license))
            {
                using (var fileStream = new FileStream(license, FileMode.Open))
                {
                    new License().SetLicense(fileStream);
                }

                return true;
            }

            return false;
        }
    }
}