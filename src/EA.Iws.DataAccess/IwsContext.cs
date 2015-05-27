namespace EA.Iws.DataAccess
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain;
    using Domain.Notification;
    using Prsd.Core.DataAccess.Extensions;
    using Prsd.Core.Domain;
    using Prsd.Core.Domain.Auditing;

    public class IwsContext : DbContext
    {
        private readonly IUserContext userContext;

        public virtual DbSet<AuditLog> AuditLogs { get; set; }

        public virtual DbSet<Country> Countries { get; set; }

        public virtual DbSet<Organisation> Organisations { get; set; }

        public virtual DbSet<NotificationApplication> NotificationApplications { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public IwsContext(IUserContext userContext)
            : base("name=Iws.DefaultConnection")
        {
            this.userContext = userContext;
            Database.SetInitializer<IwsContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var assembly = typeof(IwsContext).Assembly;
            var coreAssembly = typeof(AuditorExtensions).Assembly;

            modelBuilder.Properties<decimal>().Configure(config => config.HasPrecision(18, 4));

            modelBuilder.Conventions.AddFromAssembly(assembly);
            modelBuilder.Configurations.AddFromAssembly(assembly);

            modelBuilder.Conventions.AddFromAssembly(coreAssembly);
            modelBuilder.Configurations.AddFromAssembly(coreAssembly);
        }

        public override int SaveChanges()
        {
            this.SetEntityId();
            this.AuditChanges(userContext.UserId);

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            this.SetEntityId();
            this.AuditChanges(userContext.UserId);

            return base.SaveChangesAsync(cancellationToken);
        }

        public void DeleteOnCommit(Entity entity)
        {
            Entry(entity).State = EntityState.Deleted;
        }
    }
}