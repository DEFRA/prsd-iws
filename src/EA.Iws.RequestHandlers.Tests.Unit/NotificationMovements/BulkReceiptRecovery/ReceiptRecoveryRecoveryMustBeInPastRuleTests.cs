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
    using Prsd.Core;
    using RequestHandlers.NotificationMovements.BulkReceiptRecovery;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class ReceiptRecoveryRecoveryMustBeInPastRuleTests
    {
        private readonly ReceiptRecoveryRecoveryDateInFutureRule rule;
        private readonly Guid notificationId;
        private readonly INotificationApplicationRepository notificationRepo;
        private readonly IMovementRepository movementRepository;

        public ReceiptRecoveryRecoveryMustBeInPastRuleTests()
        {
            notificationRepo = A.Fake<INotificationApplicationRepository>();
            movementRepository = A.Fake<IMovementRepository>();
            rule = new ReceiptRecoveryRecoveryDateInFutureRule(notificationRepo, movementRepository);
            notificationId = Guid.NewGuid();

            A.CallTo(() => notificationRepo.GetById(notificationId)).Returns(A.Fake<NotificationApplication>());
            A.CallTo(() => movementRepository.GetAllMovements(notificationId))
                .Returns(GetRepoMovements(true, SystemTime.UtcNow.AddDays(-10)));
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

        private IEnumerable<Movement> GetRepoMovements(bool prenotified, DateTime shipmentDate)
        {
            return new List<Movement>()
            {
                new TestableMovement()
                {
                    NotificationId = notificationId,
                    Status =
                    prenotified ? Core.Movement.MovementStatus.Submitted : Core.Movement.MovementStatus.Captured,
                    Date = shipmentDate,
                    Number = 1
                },

                new TestableMovement()
                {
                    NotificationId = notificationId,
                    Status = Core.Movement.MovementStatus.Submitted,
                    Date = shipmentDate,
                    Number = 2
                }
            };
        }
    }
}
