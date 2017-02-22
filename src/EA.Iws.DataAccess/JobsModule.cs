namespace EA.Iws.DataAccess
{
    using Autofac;
    using Jobs;

    public class JobsModule : Module
    {
        private readonly string connectionString;

        public JobsModule(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new UpdateReportCache(connectionString)).AsSelf();
        }
    }
}