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

            builder.Register(c =>
            {
                return (c.Resolve<AppConfiguration>()).UseLocalScan ? LocalVirusScanner(c) : ClientVirusScanner(c);
            }).As<IVirusScanner>().SingleInstance();

            builder.RegisterType<FileReader>().As<IFileReader>();
        }

        private static IVirusScanner LocalVirusScanner(IComponentContext context)
        {
            var config = context.Resolve<AppConfiguration>();
            return new WriteFileVirusScanner(config.FileSafeTimerMilliseconds, context.Resolve<IFileAccess>(), config.FileUploadTempPath);
        }

        private static IVirusScanner ClientVirusScanner(IComponentContext context)
        {
            var config = context.Resolve<AppConfiguration>();
            var path = string.Empty;
            if (!string.IsNullOrWhiteSpace(config.AvCertPath))
            {
                path = System.Web.Hosting.HostingEnvironment.MapPath(config.AvCertPath);
            }
            return new IwsScanClient(config.ScanUrl, path);
        }
    }
}