namespace EA.Iws.RequestHandlers.Tests.Unit
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using DataAccess;
    using EA.Iws.RequestHandlers.Tests.Unit.Helpers;
    using FakeItEasy;
    using Prsd.Core.Domain;

    public class TestImportNotificationContext : ImportNotificationContext
    {
        public static Guid UserId = new Guid("FCB1DFC6-A7FE-4768-B979-E57AF99EBAC5");

        public TestImportNotificationContext() : base(new TestUserContext(UserId), A.Fake<IEventDispatcher>())
        {
            Initialize();
        }

        public TestImportNotificationContext(IUserContext userContext) : base(userContext, A.Fake<IEventDispatcher>())
        {
            Initialize();
        }

        public void Initialize()
        {
            this.ImportMovements = new TestDbSet<Domain.ImportMovement.ImportMovement>();
            this.ImportNotifications = new TestDbSet<Domain.ImportNotification.ImportNotification>();
            this.ImportMovementReceipts = new TestDbSet<Domain.ImportMovement.ImportMovementReceipt>();
            this.ImportMovementRejections = new TestDbSet<Domain.ImportMovement.ImportMovementRejection>();
            this.ImportMovementCompletedReceipts = new TestDbSet<Domain.ImportMovement.ImportMovementCompletedReceipt>();
            this.ImportMovementAudits = new TestDbSet<Domain.ImportMovement.ImportMovementAudit>();
            this.Importers = new TestDbSet<Domain.ImportNotification.Importer>();
            this.Exporters = new TestDbSet<Domain.ImportNotification.Exporter>();
            this.Producers = new TestDbSet<Domain.ImportNotification.Producer>();
            this.OperationCodes = new TestDbSet<Domain.ImportNotification.WasteOperation>();
            this.Facilities = new TestDbSet<Domain.ImportNotification.FacilityCollection>();
            this.Shipments = new TestDbSet<Domain.ImportNotification.Shipment>();
            this.TransportRoutes = new TestDbSet<Domain.ImportNotification.TransportRoute>();
            this.WasteTypes = new TestDbSet<Domain.ImportNotification.WasteType>();
            this.ImportNotificationAssessments = new TestDbSet<Domain.ImportNotificationAssessment.ImportNotificationAssessment>();
            this.ImportNotificationTransactions = new TestDbSet<Domain.ImportNotificationAssessment.Transactions.ImportNotificationTransaction>();
            this.ImportConsents = new TestDbSet<Domain.ImportNotificationAssessment.Consent.ImportConsent>();
            this.ImportWithdrawals = new TestDbSet<Domain.ImportNotificationAssessment.Decision.ImportWithdrawn>();
            this.ImportObjections = new TestDbSet<Domain.ImportNotificationAssessment.Decision.ImportObjection>();
            this.InterimStatuses = new TestDbSet<Domain.ImportNotification.InterimStatus>();
            this.ImportFinancialGuarantees = new TestDbSet<Domain.ImportNotificationAssessment.FinancialGuarantee.ImportFinancialGuarantee>();
            this.ImportFinancialGuaranteeApprovals = new TestDbSet<Domain.ImportNotificationAssessment.FinancialGuarantee.ImportFinancialGuaranteeApproval>();
            this.ImportFinancialGuaranteeRefusals = new TestDbSet<Domain.ImportNotificationAssessment.FinancialGuarantee.ImportFinancialGuaranteeRefusal>();
            this.ImportFinancialGuaranteeReleases = new TestDbSet<Domain.ImportNotificationAssessment.FinancialGuarantee.ImportFinancialGuaranteeRelease>();
            this.Consultations = new TestDbSet<Domain.ImportNotificationAssessment.Consultation>();
            this.NumberOfShipmentsHistories = new TestDbSet<Domain.ImportNotification.NumberOfShipmentsHistory>();
            this.ImportNotificationComments = new TestDbSet<Domain.ImportNotificationAssessment.ImportNotificationComment>();
        }

        public int SaveChangesCount { get; private set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            SaveChangesCount++;
            return Task.FromResult(1);
        }

        public override Task<int> SaveChangesAsync()
        {
            SaveChangesCount++;
            return Task.FromResult(1);
        }

        public override int SaveChanges()
        {
            SaveChangesCount++;
            return 1;
        }
    }
}
