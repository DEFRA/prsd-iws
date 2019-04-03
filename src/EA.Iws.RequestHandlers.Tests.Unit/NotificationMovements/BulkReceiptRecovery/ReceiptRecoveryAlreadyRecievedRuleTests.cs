namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.BulkReceiptRecovery
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Rules;
    using Domain.Movement;
    using FakeItEasy;
    using RequestHandlers.NotificationMovements.BulkReceiptRecovery;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class ReceiptRecoveryAlreadyRecievedRuleTests
    {
        private readonly ReceiptRecoveryAlreadyReceivedRule rule;
        private readonly Guid notificationId;

        private readonly IMovementRepository movementRepo;

        public ReceiptRecoveryAlreadyRecievedRuleTests()
        {
            this.movementRepo = A.Fake<IMovementRepository>();

            rule = new ReceiptRecoveryAlreadyReceivedRule(this.movementRepo);
            notificationId = Guid.NewGuid();
        }

        [Fact]
        public async Task NotAlreadyRecieved_Success()
        {
            A.CallTo(() => movementRepo.GetAllMovements(notificationId)).Returns(GetRepoMovements(false));
            var result = await rule.GetResult(GetTestData(), notificationId);

            Assert.Equal(ReceiptRecoveryContentRules.AlreadyRecievedRecoveredDisposed, result.Rule);
            Assert.Equal(MessageLevel.Success, result.MessageLevel);
        }

        [Fact]
        public async Task AlreadyRecieved_Fail()
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
                    ReceivedDate = DateTime.Now
                },
                new ReceiptRecoveryMovement()
                {
                    ShipmentNumber = 2,
                    ReceivedDate = DateTime.Now
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
