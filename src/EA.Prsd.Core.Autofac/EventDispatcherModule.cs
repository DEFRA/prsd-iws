namespace EA.Prsd.Core.Autofac
{
    using Domain;
    using global::Autofac;

    public class EventDispatcherModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AutofacDeferredEventDispatcher>().As<IDeferredEventDispatcher>();
        }
    }
}
