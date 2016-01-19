namespace EA.Iws.Web.Modules
{
    using Autofac;
    using Infrastructure;
    using Infrastructure.VirusScanning;

    public class FileAccessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WriteFileVirusScanner>().As<IVirusScanner>();
            builder.RegisterType<FileReader>().As<IFileReader>();
            builder.RegisterType<FileAccess>().As<IFileAccess>();
        }
    }
}