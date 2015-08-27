namespace EA.Iws.Domain.Tests.Unit.Movement
{
    using System;
    using Domain.Movement;
    using Prsd.Core;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class UpdateMovementTests : IDisposable
    {
        private static readonly Guid UserId = new Guid("35745EEC-55E7-42F1-9D8E-3515AC6FA281");
        private static readonly Guid NotificationId = new Guid("28760D3F-E18F-4986-BC7E-06BCD72D554C");
        private static readonly DateTime startDate = new DateTime(2015, 1, 1);
        private static readonly DateTime endDate = new DateTime(2016, 1, 1);

        private readonly TestableNotificationApplication notificationApplication;
        private readonly TestableShipmentInfo shipmentInfo;

        public UpdateMovementTests()
        {
            shipmentInfo = new TestableShipmentInfo
            {
                FirstDate = startDate,
                LastDate = endDate
            };

            notificationApplication = new TestableNotificationApplication
            {
                Id = NotificationId,
                UserId = UserId,
                ShipmentInfo = shipmentInfo
            };

            SystemTime.Freeze(new DateTime(2015, 06, 01));
        }

        public void Dispose()
        {
            SystemTime.Unfreeze();
        }

        [Fact]
        public void DateCannotBeBeforeStartDate()
        {
            var movement = new Movement(notificationApplication, 5);

            Action updateMovementDate = () => movement.UpdateDate(new DateTime(2014, 1, 1));

            Assert.Throws<InvalidOperationException>(updateMovementDate);
        }

        [Fact]
        public void DateCannotBeAfterTheEndDate()
        {
            var movement = new Movement(notificationApplication, 5);

            Action updateMovementDate = () => movement.UpdateDate(new DateTime(2017, 1, 1));

            Assert.Throws<InvalidOperationException>(updateMovementDate);
        }
    }
}
