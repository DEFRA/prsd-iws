namespace EA.Iws.DataAccess
{
    using System;
    using System.Data.Entity;
    using System.Security;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain;
    using Domain.AddressBook;
    using Domain.FinancialGuarantee;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using Domain.TransportRoute;
    using Prsd.Core.DataAccess.Extensions;
    using Prsd.Core.Domain;
    using Prsd.Core.Domain.Auditing;

    public class IwsContext : DbContext
    {
        private readonly IUserContext userContext;
        private readonly IEventDispatcher dispatcher;

        public IwsContext(IUserContext userContext, IEventDispatcher dispatcher)
            : base("name=Iws.DefaultConnection")
        {
            this.userContext = userContext;
            this.dispatcher = dispatcher;
            Database.SetInitializer<IwsContext>(null);
        }

        public virtual DbSet<AuditLog> AuditLogs { get; set; }

        public virtual DbSet<Country> Countries { get; set; }

        public virtual DbSet<Organisation> Organisations { get; set; }

        public virtual DbSet<NotificationApplication> NotificationApplications { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<CompetentAuthority> CompetentAuthorities { get; set; }

        public virtual DbSet<EntryOrExitPoint> EntryOrExitPoints { get; set; }

        public virtual DbSet<WasteCode> WasteCodes { get; set; }

        public virtual DbSet<PricingStructure> PricingStructures { get; set; }

        public virtual DbSet<NotificationAssessment> NotificationAssessments { get; set; }

        public virtual DbSet<NotificationDecision> NotificationDecisions { get; set; }

        public virtual DbSet<FinancialGuarantee> FinancialGuarantees { get; set; }

        public virtual DbSet<BankHoliday> BankHolidays { get; set; }

        public virtual DbSet<UnitedKingdomCompetentAuthority> UnitedKingdomCompetentAuthorities { get; set; }

        public virtual DbSet<LocalArea> LocalAreas { get; set; }

        public virtual DbSet<InternalUser> InternalUsers { get; set; }

        public virtual DbSet<Movement> Movements { get; set; }

        public virtual DbSet<UserAddress> Addresses { get; set; }

        public virtual DbSet<AddressBook> AddressBooks { get; set; }

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

            int result = base.SaveChanges();

            Task.Run(() => this.DispatchEvents(dispatcher)).Wait();

            return result;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            this.SetEntityId();
            this.DeleteRemovedRelationships();
            this.AuditChanges(userContext.UserId);

            int result = await base.SaveChangesAsync(cancellationToken);

            await this.DispatchEvents(dispatcher);

            return result;
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
                throw new SecurityException(string.Format("Access denied to this notification {0} for user {1}",
                    notificationId, userContext.UserId));
            }
            return notification;
        }
    }
}