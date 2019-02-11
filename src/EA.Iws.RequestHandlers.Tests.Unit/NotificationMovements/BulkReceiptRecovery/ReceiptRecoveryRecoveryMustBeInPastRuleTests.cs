namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.BulkReceiptRecovery
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Rules;
    using Domain;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using FakeItEasy;
    using RequestHandlers.NotificationMovements.BulkReceiptRecovery;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class ReceiptRecoveryRecoveryMustBeInPastRuleTests
    {
        private readonly ReceiptRecoveryRecoveryDateInFutureRule rule;
        private readonly Guid notificationId;
        private readonly INotificationApplicationRepository notificationRepo;

        public ReceiptRecoveryRecoveryMustBeInPastRuleTests()
        {
            notificationRepo = A.Fake<INotificationApplicationRepository>();
            rule = new ReceiptRecoveryRecoveryDateInFutureRule(notificationRepo);
            notificationId = Guid.NewGuid();

            A.CallTo(() => notificationRepo.GetById(notificationId)).Returns(A.Fake<NotificationApplication>());
        }

        [Fact]
        public async Task RecoveryDate_InPast_Success()
        {
            var result = await rule.GetResult(GetTestData(DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-1)), notificationId);

            Assert.Equal(ReceiptRecoveryContentRules.RecoveryDateValidation, result.Rule);
            Assert.Equal(MessageLevel.Success, result.MessageLevel);
        }

        [Fact]
        public async Task RecoveryDate_InFuture_Failure()
        {
            var result = await rule.GetResult(GetTestData(DateTime.Now.AddDays(1), DateTime.Now.AddDays(1)), notificationId);

            Assert.Equal(ReceiptRecoveryContentRules.RecoveryDateValidation, result.Rule);
            Assert.Equal(MessageLevel.Error, result.MessageLevel);
        }

        [Fact]
        public async Task RecoveryDate_InPast_BeforeReceiptDate_Failure()
        {
            var result = await rule.GetResult(GetTestData(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(-2)), notificationId);

            Assert.Equal(ReceiptRecoveryContentRules.RecoveryDateValidation, result.Rule);
            Assert.Equal(MessageLevel.Error, result.MessageLevel);
        }

        private List<ReceiptRecoveryMovement> GetTestData(DateTime receivedDate, DateTime recoveredDate)
        {
            return new List<ReceiptRecoveryMovement>()
            {
                new ReceiptRecoveryMovement()
                {
                    ShipmentNumber = 1,
                    ReceivedDate = receivedDate,
                    RecoveredDisposedDate = recoveredDate
                },
                new ReceiptRecoveryMovement()
                {
                    ShipmentNumber = 2,
                    ReceivedDate = receivedDate,
                    RecoveredDisposedDate = recoveredDate
                }
            };
        }
    }
}
