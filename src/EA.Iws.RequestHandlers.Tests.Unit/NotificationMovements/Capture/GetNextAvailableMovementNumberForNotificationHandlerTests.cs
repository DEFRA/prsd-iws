namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.Capture
{
    using System;
    using System.Threading.Tasks;
    using Domain.Movement;
    using FakeItEasy;
    using RequestHandlers.NotificationMovements.Capture;
    using Requests.NotificationMovements.Capture;
    using Xunit;

    public class GetNextAvailableMovementNumberForNotificationHandlerTests
    {
        [Fact]
        public async Task ReturnsNumber_BasedOnInput()
        {
            var notificationId = new Guid("13C9A673-DAAC-4738-A836-EC97B20AE1AE");

            var request = new GetNextAvailableMovementNumberForNotification(notificationId);

            var generator = A.Fake<IMovementNumberGenerator>();
            A.CallTo(() => generator.Generate(notificationId)).Returns(7);
            var requestHandler = new GetNextAvailableMovementNumberForNotificationHandler(generator);

            var result = await requestHandler.HandleAsync(request);

            Assert.Equal(7, result);
        }
    }
}
