namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.Capture
{
    using System;
    using System.Threading.Tasks;
    using Domain.Movement;
    using FakeItEasy;
    using RequestHandlers.NotificationMovements.Capture;
    using Requests.NotificationMovements.Capture;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class EnsureMovementNumberAvailableHandlerTests
    {
        private static readonly Guid NotificationId = new Guid("CD45D787-80E2-4E5E-9F1B-BD2E0F1C87CB");
        private static readonly Guid MovementId = new Guid("DD7A435F-BB74-4EA6-8680-C1811C46500A");
        private readonly GetMovementIdIfExistsHandler handler;
        private readonly IMovementRepository movementRepository;
        private Movement movement;
        
        public EnsureMovementNumberAvailableHandlerTests()
        {
            movementRepository = A.Fake<IMovementRepository>();
            
            handler = new GetMovementIdIfExistsHandler(movementRepository);
        }

        [Fact]
        public async Task ValidNumber_ReturnsMovementId()
        {
            var request = new GetMovementIdIfExists(NotificationId, 1);

            movement = new TestableMovement
            {
                Id = MovementId
            };

            A.CallTo(() => movementRepository.GetByNumberOrDefault(A<int>.Ignored, A<Guid>.Ignored)).Returns(movement);

            var result = await handler.HandleAsync(request);

            Assert.Equal(MovementId, result);
        }

        [Fact]
        public async Task InvalidNumber_ReturnsNull()
        {
            var request = new GetMovementIdIfExists(NotificationId, 2);

            A.CallTo(() => movementRepository.GetByNumberOrDefault(A<int>.Ignored, A<Guid>.Ignored)).Returns<Movement>(null);

            var result = await handler.HandleAsync(request);

            Assert.Null(result);
        }
    }
}
