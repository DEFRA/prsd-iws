namespace EA.Iws.RequestHandlers.Tests.Unit.Movement
{
    using System;
    using Core.NotificationAssessment;
    using RequestHandlers.Movement;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class NotificationMovementServiceTests
    {
        private static readonly Guid UserId = new Guid("5EC700AC-256D-4C61-ADB1-2DD481BB6741");
        private static readonly Guid NotificationId = new Guid("5EC700AC-256D-4C61-ADB1-2DD481BB6741");
        private static readonly DateTime FirstDate = new DateTime(2014, 1, 1);
        private static readonly DateTime LastDate = new DateTime(2015, 1, 1);

        private readonly NotificationMovementService movementService;
        private readonly TestableNotificationAssessment assessment;
        private readonly TestableNotificationApplication notification;
        private readonly TestableShipmentInfo shipmentInfo;
        private readonly TestIwsContext testContext;

        public NotificationMovementServiceTests()
        {
            testContext = new TestIwsContext(new TestUserContext(UserId));
            movementService = new NotificationMovementService(testContext);

            shipmentInfo = new TestableShipmentInfo
            {
                FirstDate = FirstDate,
                LastDate = LastDate
            };

            notification = new TestableNotificationApplication
            {
                Id = NotificationId,
                UserId = UserId,
                ShipmentInfo = shipmentInfo
            };

            testContext.NotificationApplications.Add(notification);

            assessment = new TestableNotificationAssessment
            {
                Id = Guid.Empty,
                NotificationApplicationId = NotificationId,
                Status = NotificationStatus.Submitted
            };

            testContext.NotificationAssessments.Add(assessment);
        }

        [Fact]
        public void NextNumbersWhereZeroReturnsOne()
        {
            Assert.Equal(1, movementService.GetNextMovementNumber(NotificationId));
        }

        [Fact]
        public void NextNumberNotificationDoesNotExistThrows()
        {
            Assert.Throws<InvalidOperationException>(() => movementService.GetNextMovementNumber(Guid.Empty));
        }

        [Fact]
        public void NextNumberWhereOneReturnsTwo()
        {
            testContext.Movements.Add(new TestableMovement
            {
                NotificationId = NotificationId
            });

            Assert.Equal(2, movementService.GetNextMovementNumber(NotificationId));
        }

        [Fact]
        public void DateInRangeReturnsTrueTest()
        {
            Assert.True(movementService.DateIsValid(NotificationId, new DateTime(2014, 5, 5)));
        }

        [Fact]
        public void DateBeforeStartReturnsFalseTest()
        {
            Assert.False(movementService.DateIsValid(NotificationId, new DateTime(2013, 5, 5)));
        }

        [Fact]
        public void DateAfterEndReturnsFalseTest()
        {
            Assert.False(movementService.DateIsValid(NotificationId, new DateTime(2015, 5, 5)));
        }
    }
}
