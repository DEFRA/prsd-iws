namespace EA.Iws.Web.Modules
{
    using Autofac;
    using Infrastructure;
    using Scanning;
    using Services;

    public class FileAccessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Scanning.FileAccess>().As<IFileAccess>();
            builder.RegisterType<ClamClientWrapper>().As<IClamClientWrapper>();

            builder.Register(c =>
            {
                var config = c.Resolve<AppConfiguration>();

                return (IVirusScanner)new WriteFileVirusScanner(config.FileSafeTimerMilliseconds, c.Resolve<IFileAccess>(), config.FileUploadTempPath);
            }).As<IVirusScanner>().SingleInstance();

            builder.Register(c =>
            {
                var config = c.Resolve<AppConfiguration>();
                var scanner = c.Resolve<IVirusScanner>();

                return (IWriteFileVirusWrapper)new WriteFileVirusWrapper(scanner, c.Resolve<IIwsScanClient>(), config.UseLocalScan);
            }).As<IWriteFileVirusWrapper>().InstancePerRequest();

            builder.RegisterType<FileReader>().As<IFileReader>();
        }
    }
}