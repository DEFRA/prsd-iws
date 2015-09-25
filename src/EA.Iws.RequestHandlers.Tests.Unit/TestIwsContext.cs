namespace EA.Iws.RequestHandlers.Tests.Unit
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.FinancialGuarantee;
    using Domain.NotificationApplication;
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
            this.FinancialGuarantees = new TestDbSet<FinancialGuarantee>();
            this.InternalUsers = new TestDbSet<InternalUser>();
            this.LocalAreas = new TestDbSet<LocalArea>();
            this.NotificationApplications = new TestDbSet<NotificationApplication>();
            this.NotificationAssessments = new TestDbSet<NotificationAssessment>();
            this.NotificationDecisions = new TestDbSet<NotificationDecision>();
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
            this.RecoveryInfos = new TestDbSet<Domain.NotificationApplication.RecoveryInfo>();
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