namespace EA.Iws.Api.Modules
{
    using Autofac;
    using Prsd.Core.Autofac;
    using Prsd.Core.Mediator;

    public class MediatrModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AutofacMediator>().As<IMediator>();
        }
    }
}