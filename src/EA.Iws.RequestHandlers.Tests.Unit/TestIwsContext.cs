namespace EA.Iws.RequestHandlers.Tests.Unit
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.Finance;
    using Domain.FinancialGuarantee;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Exporter;
    using Domain.NotificationApplication.Importer;
    using Domain.NotificationApplication.Shipment;
    using Domain.NotificationAssessment;
    using Domain.TransportRoute;
    using FakeItEasy;
    using Helpers;
    using Prsd.Core.Domain;
    using Prsd.Core.Domain.Auditing;

    internal class TestIwsContext : IwsContext
    {
        public static Guid UserId = new Guid("FCB1DFC6-A7FE-4768-B979-E57AF99EBAC5");

        public TestIwsContext() : base(new TestUserContext(UserId), A.Fake<IEventDispatcher>())
        {
            Initialize();
        }

        public TestIwsContext(IUserContext userContext) : base(userContext, A.Fake<IEventDispatcher>())
        {
            Initialize();
        }

        private void Initialize()
        {
            this.AuditLogs = new TestDbSet<AuditLog>();
            this.BankHolidays = new TestDbSet<BankHoliday>();
            this.CompetentAuthorities = new TestDbSet<CompetentAuthority>();
            this.Countries = new TestDbSet<Country>();
            this.EntryOrExitPoints = new TestDbSet<EntryOrExitPoint>();
            this.Exporters = new TestDbSet<Exporter>();
            this.FinancialGuarantees = new TestDbSet<FinancialGuaranteeCollection>();
            this.InternalUsers = new TestDbSet<InternalUser>();
            this.LocalAreas = new TestDbSet<LocalArea>();
            this.NotificationApplications = new TestDbSet<NotificationApplication>();
            this.NotificationAssessments = new TestDbSet<NotificationAssessment>();
            this.Organisations = new TestDbSet<Organisation>();
            this.PricingStructures = new TestDbSet<PricingStructure>();
            this.UnitedKingdomCompetentAuthorities = new TestDbSet<UnitedKingdomCompetentAuthority>();
            this.Users = new TestDbSet<User>();
            this.Addresses = new TestDbSet<UserAddress>();
            this.WasteCodes = new TestDbSet<WasteCode>();
            this.Movements = new TestDbSet<Domain.Movement.Movement>();
            this.AddressBooks = new TestDbSet<Domain.AddressBook.AddressBook>();
            this.TransportRoutes = new TestDbSet<Domain.TransportRoute.TransportRoute>();
            this.ShipmentInfos = new TestDbSet<ShipmentInfo>();
            this.WasteRecoveries = new TestDbSet<Domain.NotificationApplication.WasteRecovery.WasteRecovery>();
            this.Importers = new TestDbSet<Importer>();
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