namespace EA.Iws.Virus.Api.Modules
{
    using Autofac;
    using Scanning;
    using Services;

    public class FileAccessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FileAccess>().As<IFileAccess>();

            builder.Register(c =>
            {
                var config = c.Resolve<AppConfiguration>();

                return (IVirusScanner)new WriteFileVirusScanner(config.FileSafeTimerMilliseconds, c.Resolve<IFileAccess>(), config.FileUploadTempPath);
            }).As<IVirusScanner>().SingleInstance();
        }
    }
}