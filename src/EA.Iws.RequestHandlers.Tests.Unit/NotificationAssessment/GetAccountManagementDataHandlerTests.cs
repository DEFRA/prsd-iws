namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationAssessment
{
    using Core.NotificationAssessment;
    using Core.Shared;
    using Domain.NotificationAssessment;
    using EA.Iws.Domain.NotificationApplication;
    using FakeItEasy;
    using Prsd.Core.Mapper;
    using RequestHandlers.NotificationAssessment;
    using Requests.NotificationAssessment;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class GetAccountManagementDataHandlerTests
    {
        private readonly GetAccountManagementDataHandler handler;
        private readonly INotificationTransactionRepository repository;
        private readonly IMap<IList<NotificationTransaction>, AccountManagementData> accountManagementMap;
        private readonly INotificationChargeCalculator chargeCalculator;
        private readonly INotificationTransactionCalculator transactionCalculator;
        private readonly IGovUkPaySessionRepository govUkPaySessionRepository;

        private readonly Guid notificationId = Guid.NewGuid();

        public GetAccountManagementDataHandlerTests()
        {
            repository = A.Fake<INotificationTransactionRepository>();
            accountManagementMap = A.Fake<IMap<IList<NotificationTransaction>, AccountManagementData>>();
            chargeCalculator = A.Fake<INotificationChargeCalculator>();
            transactionCalculator = A.Fake<INotificationTransactionCalculator>();
            govUkPaySessionRepository = A.Fake<IGovUkPaySessionRepository>();

            A.CallTo(() => repository.GetTransactions(notificationId)).Returns(new List<NotificationTransaction>());
            A.CallTo(() => accountManagementMap.Map(A<IList<NotificationTransaction>>.Ignored)).Returns(new AccountManagementData
            {
                PaymentHistory = new List<TransactionRecordData>()
            });
            A.CallTo(() => govUkPaySessionRepository.GetFailedByNotificationId(notificationId)).Returns((IList<GovUkPaySession>)null);

            handler = new GetAccountManagementDataHandler(repository, accountManagementMap, chargeCalculator,
                transactionCalculator, govUkPaySessionRepository);
        }

        [Fact]
        public async Task HandleAsync_NoFailedSessions_ReturnsOnlyMappedTransactions()
        {
            var result = await handler.HandleAsync(new GetAccountManagementData(notificationId));

            Assert.Empty(result.PaymentHistory);
        }

        [Fact]
        public async Task HandleAsync_WithFailedSession_AddsFailedEntryToPaymentHistory()
        {
            var failedSession = new GovUkPaySession(notificationId, "payment-id", "GB1234567",
                Guid.NewGuid().ToString("N"), 50m, Guid.NewGuid(), DateTime.UtcNow);
            failedSession.UpdateStatus("failed", true, DateTime.UtcNow.AddMinutes(5));

            A.CallTo(() => govUkPaySessionRepository.GetFailedByNotificationId(notificationId))
                .Returns(new List<GovUkPaySession> { failedSession });

            var result = await handler.HandleAsync(new GetAccountManagementData(notificationId));

            var entry = Assert.Single(result.PaymentHistory);
            Assert.Equal(TransactionType.Failed, entry.Transaction);
            Assert.Equal(PaymentMethod.GovPay, entry.Type);
            Assert.Equal(50m, entry.Amount);
            Assert.Equal("GB1234567", entry.ReceiptNumber);
        }

        [Fact]
        public async Task HandleAsync_PaymentHistoryIsOrderedByDate()
        {
            var earlierSession = new GovUkPaySession(notificationId, "payment-id-1", "GB1111111",
                Guid.NewGuid().ToString("N"), 10m, Guid.NewGuid(), DateTime.UtcNow.AddDays(-2));
            earlierSession.UpdateStatus("failed", true, DateTime.UtcNow.AddDays(-2));

            var laterSession = new GovUkPaySession(notificationId, "payment-id-2", "GB2222222",
                Guid.NewGuid().ToString("N"), 20m, Guid.NewGuid(), DateTime.UtcNow.AddDays(-1));
            laterSession.UpdateStatus("failed", true, DateTime.UtcNow.AddDays(-1));

            A.CallTo(() => accountManagementMap.Map(A<IList<NotificationTransaction>>.Ignored)).Returns(new AccountManagementData
            {
                PaymentHistory = new List<TransactionRecordData>
                {
                    new TransactionRecordData { Date = DateTime.UtcNow, Transaction = TransactionType.Payment }
                }
            });

            A.CallTo(() => govUkPaySessionRepository.GetFailedByNotificationId(notificationId))
                .Returns(new List<GovUkPaySession> { laterSession, earlierSession });

            var result = await handler.HandleAsync(new GetAccountManagementData(notificationId));

            Assert.Equal(3, result.PaymentHistory.Count);
            Assert.True(result.PaymentHistory.SequenceEqual(result.PaymentHistory.OrderBy(t => t.Date)));
        }

        [Fact]
        public async Task HandleAsync_SetsTotalBillableAndBalance()
        {
            A.CallTo(() => chargeCalculator.GetValue(notificationId)).Returns(500m);
            A.CallTo(() => transactionCalculator.TotalPaid(notificationId)).Returns(200m);

            var result = await handler.HandleAsync(new GetAccountManagementData(notificationId));

            Assert.Equal(500m, result.TotalBillable);
            Assert.Equal(200m, result.Balance);
        }
    }
}