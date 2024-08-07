﻿namespace EA.Iws.DataAccess
{
    using Domain;
    using Domain.AddressBook;
    using Domain.Finance;
    using Domain.FinancialGuarantee;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Annexes;
    using Domain.NotificationApplication.Exporter;
    using Domain.NotificationApplication.Shipment;
    using Domain.NotificationApplication.WasteRecovery;
    using Domain.NotificationAssessment;
    using Domain.NotificationConsent;
    using Domain.TransportRoute;
    using Mappings.Exports;
    using Prsd.Core.Domain;
    using Prsd.Core.Domain.Auditing;
    using System;
    using System.Data.Entity;
    using System.Security;
    using System.Threading.Tasks;
    using Domain.Movement.BulkUpload;

    public class IwsContext : ContextBase
    {
        public IwsContext(IUserContext userContext, IEventDispatcher dispatcher)
            : base(userContext, dispatcher)
        {
            Database.SetInitializer<IwsContext>(null);
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

        public virtual DbSet<IntraCountryExportAllowed> IntraCountryExportAllowed { get; set; }

        public virtual DbSet<EntryOrExitPoint> EntryOrExitPoints { get; set; }

        public virtual DbSet<WasteCode> WasteCodes { get; set; }

        public virtual DbSet<PricingStructure> PricingStructures { get; set; }

        public virtual DbSet<PricingFixedFee> PricingFixedFees { get; set; }

        public virtual DbSet<NotificationAssessment> NotificationAssessments { get; set; }

        public virtual DbSet<FinancialGuaranteeCollection> FinancialGuarantees { get; set; }

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

        public virtual DbSet<WasteComponentInfo> WasteComponentInfos { get; set; }

        public virtual DbSet<Consent> Consents { get; set; }

        public virtual DbSet<Exporter> Exporters { get; set; }

        public virtual DbSet<NotificationTransaction> NotificationTransactions { get; set; }

        public virtual DbSet<MovementDetails> MovementDetails { get; set; }

        public virtual DbSet<MovementDateHistory> MovementDateHistories { get; set; }

        public virtual DbSet<MovementRejection> MovementRejections { get; set; }

        public virtual DbSet<MovementPartialRejection> MovementPartialRejections { get; set; }

        public virtual DbSet<MovementCarrier> MovementCarrier { get; set; }

        public virtual DbSet<MovementAudit> MovementAudits { get; set; }

        public virtual DbSet<AdditionalCharge> AdditionalCharges { get; set; }

        public virtual DbSet<AnnexCollection> AnnexCollections { get; set; }

        public virtual DbSet<Domain.NotificationApplication.Importer.Importer> Importers { get; set; }

        public virtual DbSet<FacilityCollection> Facilities { get; set; }

        public virtual DbSet<UserHistory> UserHistory { get; set; }

        public virtual DbSet<SharedUser> SharedUser { get; set; }

        public virtual DbSet<SharedUserHistory> SharedUserHistory { get; set; }

        public virtual DbSet<Audit> NotificationAudit { get; set; }

        public virtual DbSet<AuditScreen> NotificationAuditScreens { get; set; }

        public virtual DbSet<CarrierCollection> Carriers { get; set; }

        public virtual DbSet<ProducerCollection> Producers { get; set; }

        public virtual DbSet<MeansOfTransport> MeansOfTransports { get; set; }

        public virtual DbSet<Consultation> Consultations { get; set; }

        public virtual DbSet<TechnologyEmployed> TechnologiesEmployed { get; set; }

        public virtual DbSet<NumberOfShipmentsHistory> NumberOfShipmentsHistories { get; set; }

        public virtual DbSet<DraftBulkUpload> DraftBulkUploads { get; set; }

        public virtual DbSet<DraftMovement> DraftMovements { get; set; }

        public virtual DbSet<DraftPackagingInfo> DraftPackagingInfos { get; set; }

        public virtual DbSet<NotificationComment> NotificationComments { get; set; }

        public virtual DbSet<SystemSetting> SystemSettings { get; set; }

        public async Task<NotificationApplication> GetNotificationApplication(Guid notificationId)
        {
            //TODO: Remove this method and replace usages with repositories

            var notification = await NotificationApplications.SingleAsync(n => n.Id == notificationId);
            if (!(await IsInternal()) && notification.UserId != UserContext.UserId && !(await IsSharedUser(notificationId)))
            {
                throw new SecurityException(string.Format("Access denied to this notification {0} for user {1}",
                    notificationId, UserContext.UserId));
            }
            return notification;
        }

        public async Task<bool> IsInternal()
        {
            return await InternalUsers.AnyAsync(u => u.UserId == UserContext.UserId.ToString());
        }

        private async Task<bool> IsSharedUser(Guid notificationId)
        {
            return
                await
                    SharedUser.AnyAsync(
                        u => u.NotificationId == notificationId && u.UserId == UserContext.UserId.ToString());
        }
    }
}