namespace EA.Iws.Web.Modules
{
    using Autofac;

    public class MappingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register the map classes
            builder.RegisterAssemblyTypes(this.GetType().Assembly)
                .Where(t => t.Namespace.Contains("Mappings"))
                .AsImplementedInterfaces();
        }
    }
}