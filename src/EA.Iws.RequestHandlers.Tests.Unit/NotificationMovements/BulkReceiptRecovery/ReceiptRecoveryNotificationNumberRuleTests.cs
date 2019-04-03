namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.BulkReceiptRecovery
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Rules;
    using Domain.NotificationApplication;
    using FakeItEasy;
    using RequestHandlers.NotificationMovements.BulkReceiptRecovery;
    using Xunit;

    public class ReceiptRecoveryNotificationNumberRuleTests
    {
        private readonly ReceiptRecoveryNotificationNumberRule rule;
        private readonly INotificationApplicationRepository repository;
        private readonly Guid notificationId;
        private readonly string notificationNumber;

        public ReceiptRecoveryNotificationNumberRuleTests()
        {
            repository = A.Fake<INotificationApplicationRepository>();

            rule = new ReceiptRecoveryNotificationNumberRule(repository);
            notificationId = Guid.NewGuid();
            notificationNumber = Guid.NewGuid().ToString();
        }

        [Fact]
        public async Task GetResult_NotificationNumber_Success()
        {
            A.CallTo(() => repository.GetNumber(notificationId))
                .Returns(notificationNumber);

            var result = await rule.GetResult(GetTestData(), notificationId);

            Assert.Equal(ReceiptRecoveryContentRules.WrongNotificationNumber, result.Rule);
            Assert.Equal(MessageLevel.Success, result.MessageLevel);
        }

        [Fact]
        public async Task GetResult_NotificationNumber_Error()
        {
            A.CallTo(() => repository.GetNumber(notificationId))
                .Returns(Guid.NewGuid().ToString());

            var result = await rule.GetResult(GetTestData(), notificationId);

            Assert.Equal(ReceiptRecoveryContentRules.WrongNotificationNumber, result.Rule);
            Assert.Equal(MessageLevel.Error, result.MessageLevel);
        }

        private List<ReceiptRecoveryMovement> GetTestData()
        {
            return new List<ReceiptRecoveryMovement>()
            {
                new ReceiptRecoveryMovement()
                {
                    ShipmentNumber = 1,
                    NotificationNumber = notificationNumber
                },
                new ReceiptRecoveryMovement()
                {
                    ShipmentNumber = 2,
                    NotificationNumber = notificationNumber
                }
            };
        }
    }
}
