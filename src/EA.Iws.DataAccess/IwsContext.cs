namespace EA.Iws.DataAccess
{
    using System;
    using System.Data.Entity;
    using System.Security;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain;
    using Domain.Notification;
    using Domain.TransportRoute;
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

        public virtual DbSet<CompetentAuthority> CompetentAuthorities { get; set; }

        public virtual DbSet<EntryOrExitPoint> EntryOrExitPoints { get; set; }

        public virtual DbSet<WasteCode> WasteCodes { get; set; }

        public virtual DbSet<PricingStructure> PricingStructures { get; set; } 

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

            modelBuilder.Conventions.AddFromAssembly(assembly);
            modelBuilder.Configurations.AddFromAssembly(assembly);

            modelBuilder.Conventions.AddFromAssembly(coreAssembly);
            modelBuilder.Configurations.AddFromAssembly(coreAssembly);
        }

        public override int SaveChanges()
        {
            this.SetEntityId();
            this.DeleteRemovedRelationships();
            this.AuditChanges(userContext.UserId);

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            this.SetEntityId();
            this.DeleteRemovedRelationships();
            this.AuditChanges(userContext.UserId);

            return base.SaveChangesAsync(cancellationToken);
        }

        public void DeleteOnCommit(Entity entity)
        {
            Entry(entity).State = EntityState.Deleted;
        }

        public async Task<NotificationApplication> GetNotificationApplication(Guid notificationId)
        {
            var notification = await NotificationApplications.SingleAsync(n => n.Id == notificationId);
            if (notification.UserId != userContext.UserId)
            {
                throw new SecurityException(string.Format("Access denied to this notification {0} for user {1}", notificationId, userContext.UserId));
            }
            return notification;
        }
    }
}