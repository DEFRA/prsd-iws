namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.BulkReceiptRecovery
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Rules;
    using Domain;
    using Domain.Movement;
    using FakeItEasy;
    using RequestHandlers.NotificationMovements.BulkReceiptRecovery;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class ReceiptRecoveryReceiptOnlyRuleTests
    {
        private readonly ReceiptRecoveryReceiptOnlyRule rule;
        private readonly Guid notificationId;
        private readonly IMovementRepository repo;

        public ReceiptRecoveryReceiptOnlyRuleTests()
        {
            repo = A.Fake<IMovementRepository>();
            rule = new ReceiptRecoveryReceiptOnlyRule(repo);
            notificationId = Guid.NewGuid();
        }

        [Fact]
        public async Task RecieptOnly_Prenotified_Success()
        {
            A.CallTo(() => repo.GetAllMovements(notificationId)).Returns(GetRepoMovements(true, DateTime.Now));
            var result = await rule.GetResult(GetTestData(false), notificationId);

            Assert.Equal(ReceiptRecoveryContentRules.PrenotifiedShipment, result.Rule);
            Assert.Equal(MessageLevel.Success, result.MessageLevel);
        }

        [Fact]
        public async Task RecieptOnly_CapturedButDateInPast_Failure()
        {
            A.CallTo(() => repo.GetAllMovements(notificationId)).Returns(GetRepoMovements(false, DateTime.Now.AddDays(-1)));
            var result = await rule.GetResult(GetTestData(true), notificationId);

            Assert.Equal(ReceiptRecoveryContentRules.PrenotifiedShipment, result.Rule);
            Assert.Equal(MessageLevel.Error, result.MessageLevel);
        }

        [Fact]
        public async Task RecieptOnly_CapturedButDateInFuture_Success()
        {
            A.CallTo(() => repo.GetAllMovements(notificationId)).Returns(GetRepoMovements(false, DateTime.Now.AddDays(1)));
            var result = await rule.GetResult(GetTestData(true), notificationId);

            Assert.Equal(ReceiptRecoveryContentRules.PrenotifiedShipment, result.Rule);
            Assert.Equal(MessageLevel.Success, result.MessageLevel);
        }

        private List<ReceiptRecoveryMovement> GetTestData(bool recovered)
        {
            return new List<ReceiptRecoveryMovement>()
            {
                new ReceiptRecoveryMovement()
                {
                    ShipmentNumber = 1,
                    MissingRecoveredDisposedDate = recovered
                },
                new ReceiptRecoveryMovement()
                {
                    ShipmentNumber = 2
                }
            };
        }

        private List<Movement> GetRepoMovements(bool prenotified, DateTime shipmentDate)
        {
            return new List<Movement>()
            {
             new TestableMovement()
                {
                    NotificationId = notificationId,
                    Status = prenotified ? Core.Movement.MovementStatus.Received : Core.Movement.MovementStatus.Captured,
                    Number = 1,
                    Date = shipmentDate
                },

            new TestableMovement()
                {
                    NotificationId = notificationId,
                    Status = Core.Movement.MovementStatus.Submitted,
                    Number = 2,
                    Date = shipmentDate
                }
            };
        }
    }
}
