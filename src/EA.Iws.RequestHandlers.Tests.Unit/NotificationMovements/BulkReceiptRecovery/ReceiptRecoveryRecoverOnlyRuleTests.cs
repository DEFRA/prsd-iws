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

    public class ReceiptRecoveryRecoverOnlyRuleTests
    {
        private readonly ReceiptRecoveryRecoveryOnlyRule rule;
        private readonly Guid notificationId;
        private readonly IMovementRepository movementRepo;
        private readonly INotificationApplicationRepository notificationRepo;

        public ReceiptRecoveryRecoverOnlyRuleTests()
        {
            movementRepo = A.Fake<IMovementRepository>();
            notificationRepo = A.Fake<INotificationApplicationRepository>();
            rule = new ReceiptRecoveryRecoveryOnlyRule(movementRepo, notificationRepo);
            notificationId = Guid.NewGuid();

            A.CallTo(() => notificationRepo.GetById(notificationId)).Returns(A.Fake<NotificationApplication>());
        }

        [Fact]
        public async Task RecoverOnly_Received_Success()
        {
            A.CallTo(() => movementRepo.GetAllMovements(notificationId)).Returns(GetRepoMovements(true));
            var result = await rule.GetResult(GetTestData(false), notificationId);

            Assert.Equal(ReceiptRecoveryContentRules.RecoveredValidation, result.Rule);
            Assert.Equal(MessageLevel.Success, result.MessageLevel);
        }

        [Fact]
        public async Task RecoverOnly_NotReceived_Failure()
        {
            A.CallTo(() => movementRepo.GetAllMovements(notificationId)).Returns(GetRepoMovements(false));
            var result = await rule.GetResult(GetTestData(true), notificationId);

            Assert.Equal(ReceiptRecoveryContentRules.RecoveredValidation, result.Rule);
            Assert.Equal(MessageLevel.Error, result.MessageLevel);
        }

        private List<ReceiptRecoveryMovement> GetTestData(bool received)
        {
            DateTime? date = null;

            if (received)
            {
                date = DateTime.Now;
            }

            return new List<ReceiptRecoveryMovement>()
            {
                new ReceiptRecoveryMovement()
                {
                    ShipmentNumber = 1,
                    MissingReceivedDate = received,
                    RecoveredDisposedDate = date
                },
                new ReceiptRecoveryMovement()
                {
                    ShipmentNumber = 2,
                    MissingReceivedDate = received,
                    RecoveredDisposedDate = date
                }
            };
        }

        private List<Movement> GetRepoMovements(bool received)
        {
            return new List<Movement>()
            {
             new TestableMovement()
                {
                    NotificationId = notificationId,
                    Status = received ? Core.Movement.MovementStatus.Received : Core.Movement.MovementStatus.Submitted,
                    Number = 1
                },

            new TestableMovement()
                {
                    NotificationId = notificationId,
                    Status = Core.Movement.MovementStatus.Submitted,
                    Number = 2
                }
            };
        }
    }
}
