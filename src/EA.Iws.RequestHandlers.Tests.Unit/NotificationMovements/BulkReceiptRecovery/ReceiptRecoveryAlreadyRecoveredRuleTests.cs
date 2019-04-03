namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.BulkReceiptRecovery
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Rules;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using FakeItEasy;
    using RequestHandlers.NotificationMovements.BulkReceiptRecovery;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class ReceiptRecoveryAlreadyRecoveredRuleTests
    {
        private readonly ReceiptRecoveryAlreadyRecoveredRule rule;
        private readonly Guid notificationId;

        private readonly IMovementRepository movementRepo;
        private readonly INotificationApplicationRepository notificationRepo;

        public ReceiptRecoveryAlreadyRecoveredRuleTests()
        {
            this.movementRepo = A.Fake<IMovementRepository>();
            this.notificationRepo = A.Fake<INotificationApplicationRepository>();

            rule = new ReceiptRecoveryAlreadyRecoveredRule(this.movementRepo, this.notificationRepo);
            notificationId = Guid.NewGuid();

            A.CallTo(() => this.notificationRepo.GetById(notificationId)).Returns(A.Fake<NotificationApplication>());
        }

        [Fact]
        public async Task NotAlreadyRecovered_Success()
        {
            A.CallTo(() => movementRepo.GetAllMovements(notificationId)).Returns(GetRepoMovements(false));
            var result = await rule.GetResult(GetTestData(), notificationId);

            Assert.Equal(ReceiptRecoveryContentRules.AlreadyRecievedRecoveredDisposed, result.Rule);
            Assert.Equal(MessageLevel.Success, result.MessageLevel);
        }

        [Fact]
        public async Task AlreadyRecovered_Fail()
        {
            A.CallTo(() => movementRepo.GetAllMovements(notificationId)).Returns(GetRepoMovements(true));
            var result = await rule.GetResult(GetTestData(), notificationId);

            Assert.Equal(ReceiptRecoveryContentRules.AlreadyRecievedRecoveredDisposed, result.Rule);
            Assert.Equal(MessageLevel.Error, result.MessageLevel);
        }

        private List<ReceiptRecoveryMovement> GetTestData()
        {
            return new List<ReceiptRecoveryMovement>()
            {
                new ReceiptRecoveryMovement()
                {
                    ShipmentNumber = 1,
                    RecoveredDisposedDate = DateTime.Now
                },
                new ReceiptRecoveryMovement()
                {
                    ShipmentNumber = 2,
                    RecoveredDisposedDate = DateTime.Now
                }
            };
        }

        private List<Movement> GetRepoMovements(bool receovered)
        {
            return new List<Movement>()
            {
             new TestableMovement()
                {
                    NotificationId = notificationId,
                    Status = receovered ? Core.Movement.MovementStatus.Completed : Core.Movement.MovementStatus.Captured,
                    Number = 1,
                    Date = DateTime.Now
                },

            new TestableMovement()
                {
                    NotificationId = notificationId,
                    Status = Core.Movement.MovementStatus.Submitted,
                    Number = 2,
                    Date = DateTime.Now
                }
            };
        }
    }
}
