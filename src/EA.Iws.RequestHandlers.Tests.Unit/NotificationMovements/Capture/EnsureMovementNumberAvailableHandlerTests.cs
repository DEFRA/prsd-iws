namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.Capture
{
    using System;
    using System.Threading.Tasks;
    using Domain.Movement;
    using FakeItEasy;
    using RequestHandlers.NotificationMovements.Capture;
    using Requests.NotificationMovements.Capture;
    using Xunit;

    public class EnsureMovementNumberAvailableHandlerTests
    {
        private static readonly Guid NotificationId = new Guid("CD45D787-80E2-4E5E-9F1B-BD2E0F1C87CB");
        private readonly EnsureMovementNumberAvailableHandler handler;
        private readonly IMovementNumberValidator validator;
        
        public EnsureMovementNumberAvailableHandlerTests()
        {
            validator = A.Fake<IMovementNumberValidator>();
            handler = new EnsureMovementNumberAvailableHandler(validator);
        }

        [Fact]
        public async Task ValidNumber_ReturnsTrue()
        {
            var request = new EnsureMovementNumberAvailable(NotificationId, 1);
            A.CallTo(() => validator.Validate(NotificationId, 1)).Returns(true);

            var result = await handler.HandleAsync(request);

            Assert.True(result);
        }

        [Fact]
        public async Task InvalidNumber_ReturnsFalse()
        {
            var request = new EnsureMovementNumberAvailable(NotificationId, 2);
            A.CallTo(() => validator.Validate(NotificationId, 2)).Returns(false);

            var result = await handler.HandleAsync(request);

            Assert.False(result);
        }
    }
}
