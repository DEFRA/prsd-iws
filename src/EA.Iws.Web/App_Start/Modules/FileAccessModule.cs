namespace EA.Iws.Web.Modules
{
    using Autofac;
    using Infrastructure;
    using Infrastructure.VirusScanning;
    using Services;

    public class FileAccessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FileAccess>().As<IFileAccess>();

            builder.Register(c =>
            {
                var clamAv = c.Resolve<AppConfiguration>().ClamAvHost;

                if (!string.IsNullOrWhiteSpace(clamAv))
                {
                    return (IVirusScanner)new ClamAvVirusScanner(c.Resolve<IClamClientWrapper>());
                }
                //return new ClamAvVirusScanner(c.Resolve<IClaimClientWrapper>());
                return (IVirusScanner)new WriteFileVirusScanner(c.Resolve<AppConfiguration>(), c.Resolve<IFileAccess>());
            }).As<IVirusScanner>().SingleInstance();
            //builder.RegisterType<WriteFileVirusScanner>().As<IVirusScanner>();
            builder.RegisterType<FileReader>().As<IFileReader>();
            builder.RegisterType<FileAccess>().As<IFileAccess>();
        }
    }
}