namespace EA.Iws.RequestHandlers.Tests.Unit.Admin.FinancialGuarantee
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.FinancialGuarantee;
    using DataAccess;
    using Domain.FinancialGuarantee;
    using FakeItEasy;
    using Prsd.Core;
    using RequestHandlers.Admin.FinancialGuarantee;
    using TestHelpers.Helpers;
    using Xunit;

    public class FinancialGuaranteeStatusChangeEventHandlerTests
    {
        private const string AnyString = "test";
        private static readonly Guid UserId = new Guid("E3E92750-3BFC-4913-B152-89EC07B78CB6");

        private readonly FinancialGuaranteeStatusChangeEventHandler handler;
        private readonly IwsContext context;
        private readonly FinancialGuarantee financialGuarantee;
        private readonly FinancialGuaranteeStatusChangeEvent receivedEvent;

        public FinancialGuaranteeStatusChangeEventHandlerTests()
        {
            context = new TestIwsContext();
            var userContext = new TestUserContext(UserId);
            
            handler = new FinancialGuaranteeStatusChangeEventHandler(context, userContext);

            context.Users.Add(UserFactory.Create(UserId, AnyString, AnyString, AnyString, AnyString));

            financialGuarantee = FinancialGuarantee.Create(new Guid("68787AC6-7CF5-4862-8E7E-77E20172AECC"));

            receivedEvent = new FinancialGuaranteeStatusChangeEvent(financialGuarantee,
                FinancialGuaranteeStatus.ApplicationReceived);
        }

        [Fact]
        public async Task AddsStatusChangeRecord()
        {
            var currentRecordCount = financialGuarantee.StatusChanges.Count();

            await
                handler.HandleAsync(receivedEvent);

            Assert.Equal(currentRecordCount + 1, financialGuarantee.StatusChanges.Count());
        }

        [Fact]
        public async Task AddsStatusChangeRecord_WithCorrectStatus()
        {
            Predicate<FinancialGuaranteeStatusChange> getReceivedStatusChanges = fg => fg.Status == FinancialGuaranteeStatus.ApplicationReceived;

            Assert.DoesNotContain(financialGuarantee.StatusChanges, getReceivedStatusChanges);

            await
                handler.HandleAsync(receivedEvent);

            Assert.Contains(financialGuarantee.StatusChanges, getReceivedStatusChanges);
        }

        [Fact]
        public async Task AddsStatusChangeRecord_WithCorrectDate()
        {
            SystemTime.Freeze();

            var date = SystemTime.UtcNow;

            await handler.HandleAsync(receivedEvent);

            Assert.Equal(date, financialGuarantee.StatusChanges.Single(sc => sc.Status == FinancialGuaranteeStatus.ApplicationReceived).ChangeDate);

            SystemTime.Unfreeze();
        }

        [Fact]
        public async Task SavesChanges_IsCalled()
        {
            await handler.HandleAsync(receivedEvent);

            Assert.Equal(1, ((TestIwsContext)context).SaveChangesCount);
        }

        [Fact]
        public async Task AddsStatusChangeRecord_WithCorrectUser()
        {
            await handler.HandleAsync(receivedEvent);

            Assert.Equal(UserId.ToString(), financialGuarantee.StatusChanges.Single(sc => sc.Status == FinancialGuaranteeStatus.ApplicationReceived).User.Id);
        }
    }
}
