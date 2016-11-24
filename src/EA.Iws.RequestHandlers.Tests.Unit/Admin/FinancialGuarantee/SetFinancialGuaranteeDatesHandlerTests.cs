namespace EA.Iws.RequestHandlers.Tests.Unit.Admin.FinancialGuarantee
{
    using System;
    using System.Threading.Tasks;
    using Core.FinancialGuarantee;
    using DataAccess;
    using Domain.FinancialGuarantee;
    using FakeItEasy;
    using RequestHandlers.Admin.FinancialGuarantee;
    using Requests.Admin.FinancialGuarantee;
    using TestHelpers.Helpers;
    using Xunit;

    public class SetFinancialGuaranteeDatesHandlerTests
    {
        private static readonly Guid PendingNotificationId = new Guid("F6BB1BC8-3BD1-4CBE-8FAB-7CA3BE6274CD");
        private static readonly Guid ReceivedNotificationId = new Guid("53E57ABA-1891-4798-ADAB-60BB2E56A63D");
        private static readonly Guid PendingFinancialGuaranteeId = new Guid("B725446F-80EC-461A-A532-69F7111BCD57");
        private static readonly Guid ReceivedFinancialGuaranteeId = new Guid("EAC88B33-C51F-417A-AE09-1D06AEABEBB5");
        private static readonly DateTime AnyDate = new DateTime(2014, 5, 5);

        private readonly IwsContext context;
        private readonly IFinancialGuaranteeRepository repository;
        private readonly SetFinancialGuaranteeDatesHandler handler;
        private readonly FinancialGuarantee receivedFinancialGuarantee;
        private readonly FinancialGuarantee pendingFinancialGuarantee;

        public SetFinancialGuaranteeDatesHandlerTests()
        {
            context = new TestIwsContext();
            repository = A.Fake<IFinancialGuaranteeRepository>();

            var receivedFinancialGuaranteeCollection = new FinancialGuaranteeCollection(ReceivedNotificationId);

            receivedFinancialGuarantee = receivedFinancialGuaranteeCollection.AddFinancialGuarantee(AnyDate);
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.Status, FinancialGuaranteeStatus.ApplicationReceived,
                receivedFinancialGuarantee);
            ObjectInstantiator<FinancialGuarantee>.SetProperty(fg => fg.ReceivedDate, AnyDate,
                receivedFinancialGuarantee);
            EntityHelper.SetEntityId(receivedFinancialGuarantee, ReceivedFinancialGuaranteeId);

            var pendingFinancialGuaranteeCollection = new FinancialGuaranteeCollection(PendingNotificationId);
            pendingFinancialGuarantee = pendingFinancialGuaranteeCollection.AddFinancialGuarantee(AnyDate);
            EntityHelper.SetEntityId(pendingFinancialGuarantee, PendingFinancialGuaranteeId);

            A.CallTo(() => repository.GetByNotificationId(PendingNotificationId))
                .Returns(pendingFinancialGuaranteeCollection);

            A.CallTo(() => repository.GetByNotificationId(ReceivedNotificationId))
                .Returns(receivedFinancialGuaranteeCollection);

            handler = new SetFinancialGuaranteeDatesHandler(repository, context);
        }

        [Fact]
        public void Request_CannotBeInstantiated_WithNullDates()
        {
            Assert.Throws<ArgumentException>(() => new SetFinancialGuaranteeDates(Guid.Empty, Guid.Empty, null, null));
        }

        [Fact]
        public void Request_CanBeInstantiated_WithNullReceivedDate()
        {
            var result = new SetFinancialGuaranteeDates(Guid.Empty, Guid.Empty, null, AnyDate);

            Assert.Equal(AnyDate, result.CompletedDate);
        }

        [Fact]
        public void Request_CanBeInstantiated_WithNullCompletedDate()
        {
            var result = new SetFinancialGuaranteeDates(Guid.Empty, Guid.Empty, AnyDate, null);

            Assert.Equal(AnyDate, result.ReceivedDate);
        }

        [Fact]
        public async Task Handler_NotificationDoesNotExist_Throws()
        {
            await
                Assert.ThrowsAsync<InvalidOperationException>(
                    () => handler.HandleAsync(new SetFinancialGuaranteeDates(Guid.Empty, Guid.Empty, AnyDate, null)));
        }

        [Fact]
        public async Task Handler_SetReceivedDateForPendingRecord_SetsDateAndStatus()
        {
            await handler.HandleAsync(new SetFinancialGuaranteeDates(PendingNotificationId, PendingFinancialGuaranteeId, AnyDate, null));

            Assert.Equal(AnyDate, pendingFinancialGuarantee.ReceivedDate);
            Assert.Equal(FinancialGuaranteeStatus.ApplicationReceived, pendingFinancialGuarantee.Status);
        }

        [Fact]
        public async Task Handler_SetReceivedDatesForPendingRecord_SavesChanges()
        {
            await handler.HandleAsync(new SetFinancialGuaranteeDates(PendingNotificationId, PendingFinancialGuaranteeId, AnyDate, null));

            Assert.Equal(1, ((TestIwsContext)context).SaveChangesCount);
        }

        [Fact]
        public async Task Handler_SetCompletedDateForReceivedRecord_SetsDateAndStatus()
        {
            await
                handler.HandleAsync(new SetFinancialGuaranteeDates(ReceivedNotificationId, ReceivedFinancialGuaranteeId, null,
                    AnyDate.AddDays(1)));

            Assert.Equal(AnyDate.AddDays(1), receivedFinancialGuarantee.CompletedDate);
            Assert.Equal(FinancialGuaranteeStatus.ApplicationComplete, receivedFinancialGuarantee.Status);
        }
    }
}