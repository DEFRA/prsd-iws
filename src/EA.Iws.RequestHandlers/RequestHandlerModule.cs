namespace EA.Iws.RequestHandlers
{
    using System;
    using System.IO;
    using System.Reflection;
    using Aspose.Words;
    using Authorization;
    using Autofac;
    using Core.Authorization;
    using Core.ComponentRegistration;
    using Core.Movement;
    using Core.Movement.Bulk;
    using Decorators;
    using Documents;
    using Domain.ImportNotification;
    using Domain.NotificationApplication;
    using ImportNotification;
    using Movement;
    using Prsd.Core.Autofac;
    using Prsd.Core.Decorators;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using Module = Autofac.Module;

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

            // Register all components with the AutoRegister attribute as self and interfaces
            builder.RegisterAssemblyTypes(ThisAssembly, typeof(NotificationApplication).Assembly)
                .Where(t => t.GetCustomAttribute<AutoRegisterAttribute>() != null)
                .AsSelf()
                .AsImplementedInterfaces();

            if (HasAsposeLicense())
            {
                builder.RegisterType<AsposePdfGenerator>().As<IPdfGenerator>();
            }

            builder.RegisterType<InMemoryAuthorizationManager>().As<IAuthorizationManager>();
            builder.RegisterType<RequestAuthorizationAttributeCache>().AsSelf().SingleInstance();

            builder.RegisterType<AddressBuilder>().InstancePerDependency().AsSelf();
            
            // Rules
            builder.RegisterAssemblyTypes(ThisAssembly)
                .AssignableTo<IMovementRule>()
                .As<IMovementRule>();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .AssignableTo<IBulkMovementPrenotificationContentRule>()
                .As<IBulkMovementPrenotificationContentRule>();
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