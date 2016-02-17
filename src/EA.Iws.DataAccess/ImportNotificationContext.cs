namespace EA.Iws.DataAccess
{
    using System.Data.Entity;
    using Domain.ImportMovement;
    using Domain.ImportNotification;
    using Domain.ImportNotificationAssessment;
    using Domain.ImportNotificationAssessment.Consent;
    using Domain.ImportNotificationAssessment.Decision;
    using Domain.ImportNotificationAssessment.FinancialGuarantee;
    using Domain.ImportNotificationAssessment.Transactions;
    using Mappings.Imports;
    using Prsd.Core.Domain;

    public class ImportNotificationContext : ContextBase
    {
        public ImportNotificationContext(IUserContext userContext, IEventDispatcher dispatcher)
            : base(userContext, dispatcher)
        {
            Database.SetInitializer<ImportNotificationContext>(null);
        }

        public virtual DbSet<ImportMovement> ImportMovements { get; set; }

        public virtual DbSet<ImportNotification> ImportNotifications { get; set; }

        public virtual DbSet<ImportMovementReceipt> ImportMovementReceipts { get; set; }

        public virtual DbSet<ImportMovementRejection> ImportMovementRejections { get; set; }

        public virtual DbSet<ImportMovementCompletedReceipt> ImportMovementCompletedReceipts { get; set; }

        public virtual DbSet<Importer> Importers { get; set; }

        public virtual DbSet<Exporter> Exporters { get; set; }

        public virtual DbSet<Producer> Producers { get; set; }

        public virtual DbSet<WasteOperation> OperationCodes { get; set; }

        public virtual DbSet<FacilityCollection> Facilities { get; set; }
       
        public virtual DbSet<Shipment> Shipments { get; set; }

        public virtual DbSet<TransportRoute> TransportRoutes { get; set; } 

        public virtual DbSet<WasteType> WasteTypes { get; set; }

        public virtual DbSet<ImportNotificationAssessment> ImportNotificationAssessments { get; set; }

        public virtual DbSet<ImportNotificationTransaction> ImportNotificationTransactions { get; set; }

        public virtual DbSet<ImportConsent> ImportConsents { get; set; }

        public virtual DbSet<ImportWithdrawn> ImportWithdrawals { get; set; }

        public virtual DbSet<ImportObjection> ImportObjections { get; set; }

        public virtual DbSet<InterimStatus> InterimStatuses { get; set; }

        public virtual DbSet<ImportFinancialGuarantee> ImportFinancialGuarantees { get; set; }

        public virtual DbSet<ImportFinancialGuaranteeApproval> ImportFinancialGuaranteeApprovals { get; set; }

        public virtual DbSet<ImportFinancialGuaranteeRefusal> ImportFinancialGuaranteeRefusals { get; set; }

        public virtual DbSet<ImportFinancialGuaranteeRelease> ImportFinancialGuaranteeReleases { get; set; }

        public virtual DbSet<Consultation> Consultations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.AddFromNamespace(typeof(ImportNotificationMapping).Namespace);
        }
    }
}
