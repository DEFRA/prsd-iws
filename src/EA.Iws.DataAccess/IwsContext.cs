namespace EA.Iws.DataAccess
{
    using System;
    using System.Data.Entity;
    using System.Security;
    using System.Threading.Tasks;
    using Domain;
    using Domain.AddressBook;
    using Domain.FinancialGuarantee;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Exporter;
    using Domain.NotificationApplication.Shipment;
    using Domain.NotificationApplication.WasteRecovery;
    using Domain.NotificationAssessment;
    using Domain.NotificationConsent;
    using Domain.TransportRoute;
    using Mappings.Exports;
    using Prsd.Core.Domain;
    using Prsd.Core.Domain.Auditing;

    public class IwsContext : ContextBase
    {
        public IwsContext(IUserContext userContext, IEventDispatcher dispatcher)
            : base(userContext, dispatcher)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.AddFromNamespace(typeof(NotificationApplicationMapping).Namespace);
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

        public virtual DbSet<ShipmentInfo> ShipmentInfos { get; set; }

        public virtual DbSet<TransportRoute> TransportRoutes { get; set; } 
        
        public virtual DbSet<WasteRecovery> WasteRecoveries { get; set; }

        public virtual DbSet<WasteDisposal> WasteDisposals { get; set; }

        public virtual DbSet<Consent> Consents { get; set; }

        public virtual DbSet<Exporter> Exporters { get; set; }

        public virtual DbSet<NotificationTransaction> NotificationTransactions { get; set; } 

        public virtual DbSet<MovementDetails> MovementDetails { get; set; }

        public virtual DbSet<MovementDateHistory> MovementDateHistories { get; set; }

        public virtual DbSet<MovementRejection> MovementRejections { get; set; }

        public virtual DbSet<Domain.NotificationApplication.Importer.Importer> Importers { get; set; }

        public async Task<NotificationApplication> GetNotificationApplication(Guid notificationId)
        {
            var notification = await NotificationApplications.SingleAsync(n => n.Id == notificationId);
            if (notification.UserId != UserContext.UserId)
            {
                throw new SecurityException(string.Format("Access denied to this notification {0} for user {1}",
                    notificationId, UserContext.UserId));
            }
            return notification;
        }
    }
}