namespace EA.Iws.RequestHandlers
{
    using System;
    using System.IO;
    using Aspose.Words;
    using Authorization;
    using Autofac;
    using Copy;
    using Core.Movement;
    using Decorators;
    using Documents;
    using Domain;
    using Domain.ImportMovement;
    using Domain.ImportNotification;
    using Domain.ImportNotificationAssessment;
    using Domain.ImportNotificationAssessment.Consent;
    using Domain.ImportNotificationAssessment.Transactions;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Annexes;
    using Domain.NotificationAssessment;
    using Domain.NotificationConsent;
    using ImportNotification;
    using ImportNotification.Summary;
    using Notification;
    using Prsd.Core.Autofac;
    using Prsd.Core.Decorators;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Validation;

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

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.IsAssignableTo<FluentValidation.IValidator>())
                .AsImplementedInterfaces();

            builder.RegisterType<NotificationToNotificationCopy>().AsSelf();
            builder.RegisterType<TransportRouteToTransportRouteCopy>().AsSelf();
            builder.RegisterType<WasteRecoveryToWasteRecoveryCopy>().AsSelf();
            builder.RegisterType<ExporterToExporterCopy>().AsSelf();
            builder.RegisterType<ImporterToImporterCopy>().AsSelf();
            builder.RegisterType<AnnexCollectionToAnnexCollectionCopy>().AsSelf();

            builder.RegisterType<WasteCodeCopy>().AsSelf();
            builder.RegisterType<MovementFactory>().AsSelf();
            builder.RegisterType<MovementDetailsFactory>().AsSelf();
            builder.RegisterType<NotificationMovementsQuantity>().AsSelf();
            builder.RegisterType<NotificationChargeCalculator>().AsSelf();
            builder.RegisterType<CertificateFactory>().AsSelf();
            builder.RegisterType<CertificateOfReceiptNameGenerator>().AsSelf();
            builder.RegisterType<CertificateOfRecoveryNameGenerator>().AsSelf();
            
            builder.RegisterType<ConsentNotification>().AsSelf();
            builder.RegisterType<MovementFileNameGenerator>().AsSelf();
            builder.RegisterType<DecisionRequiredBy>().AsSelf();
            builder.RegisterType<MovementNumberGenerator>().AsSelf();
            builder.RegisterType<NotificationTransactionCalculator>().AsSelf();
            builder.RegisterType<TransportRouteSummary>().AsSelf();
            builder.RegisterType<WasteTypeSummary>().AsSelf();
            builder.RegisterType<OriginalMovementDate>().AsSelf();
            builder.RegisterType<ValidMovementDateCalculator>().AsSelf();
            builder.RegisterType<Transaction>().AsSelf();
            builder.RegisterType<DaysRemainingCalculator>().AsSelf();
            builder.RegisterType<FinancialGuaranteeDecisionRequired>().AsSelf();
            builder.RegisterType<NumberOfMovements>().AsSelf();
            builder.RegisterType<NumberOfActiveLoads>().AsSelf();
            builder.RegisterType<ConsentPeriod>().AsSelf();

            builder.RegisterType<NotificationNumberGenerator>().As<INotificationNumberGenerator>();
            builder.RegisterType<CapturedMovementFactory>().As<ICapturedMovementFactory>();
            builder.RegisterType<WorkingDayCalculator>().As<IWorkingDayCalculator>();
            builder.RegisterType<NotificationProgressService>().As<INotificationProgressService>();
            builder.RegisterType<MovementNumberGenerator>().As<IMovementNumberGenerator>();
            builder.RegisterType<NextAvailableMovementNumberGenerator>().As<INextAvailableMovementNumberGenerator>();
            builder.RegisterType<RejectMovement>().As<IRejectMovement>();
            builder.RegisterType<NotificationChargeCalculator>().As<INotificationChargeCalculator>();
            builder.RegisterType<MovementNumberValidator>().As<IMovementNumberValidator>();
            builder.RegisterType<NotificationTransactionCalculator>().As<INotificationTransactionCalculator>();

            builder.RegisterType<MovementDateValidator>().As<IMovementDateValidator>();
            builder.RegisterType<ImportMovementFactory>().As<IImportMovementFactory>();
            builder.RegisterType<ImportMovementNumberValidator>().As<IImportMovementNumberValidator>();
            builder.RegisterType<Validator>().As<IValidator>();
            builder.RegisterType<DecisionRequiredByCalculator>().As<IDecisionRequiredByCalculator>();

            builder.RegisterType<RejectImportMovement>().As<IRejectImportMovement>();
            builder.RegisterType<ReceiveImportMovement>().As<IReceiveImportMovement>();
            builder.RegisterType<CompleteImportMovement>().As<ICompleteImportMovement>();

            if (HasAsposeLicense())
            {
                builder.RegisterType<AsposePdfGenerator>().As<IPdfGenerator>();
            }

            builder.RegisterType<AuthorizationManager>().As<IAuthorizationManager>();
            builder.RegisterType<UserRoleService>().As<IUserRoleService>();
            builder.RegisterType<RequestAuthorizationAttributeCache>().AsSelf().SingleInstance();
            builder.RegisterType<InMemoryAuthorizationService>().As<IAuthorizationService>();

            builder.RegisterType<AddressBuilder>().InstancePerDependency().AsSelf();

            RegisterImportNotificationFinance(builder);
            RegisterExportNotificationAnnexes(builder);
            RegisterImportNotificationAssessment(builder);
            
            // Rules
            builder.RegisterAssemblyTypes(ThisAssembly)
                .AssignableTo<IMovementRule>()
                .As<IMovementRule>();
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

        private void RegisterImportNotificationFinance(ContainerBuilder builder)
        {
            builder.RegisterType<ImportPaymentTransaction>().AsSelf();

            builder.RegisterType<ImportNotificationTransactionCalculator>()
                .As<IImportNotificationTransactionCalculator>();

            builder.RegisterType<ImportNotificationChargeCalculator>().As<IImportNotificationChargeCalculator>();
        }

        private void RegisterExportNotificationAnnexes(ContainerBuilder builder)
        {
            builder.RegisterType<AnnexFactory>().AsSelf();
            builder.RegisterType<ProcessOfGenerationNameGenerator>().AsSelf();
            builder.RegisterType<WasteCompositionNameGenerator>().AsSelf();
            builder.RegisterType<TechnologyEmployedNameGenerator>().AsSelf();
            builder.RegisterType<RequiredAnnexes>().AsSelf();
        }

        private void RegisterImportNotificationAssessment(ContainerBuilder builder)
        {
            builder.RegisterType<ConsentImportNotification>().AsSelf();
            builder.RegisterType<Domain.ImportNotificationAssessment.Decision.DecisionRequiredBy>().AsSelf();
            builder.RegisterType<Domain.ImportNotificationAssessment.Decision.DecisionRequiredByCalculator>().As<Domain.ImportNotificationAssessment.Decision.IDecisionRequiredByCalculator>();
        }
    }
}