namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.Capture
{
    using DataAccess;
    using Domain.Movement;
    using FakeItEasy;
    using RequestHandlers.NotificationMovements.Capture;
    using Requests.NotificationMovements.Capture;
    using System;
    using System.Threading.Tasks;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class CreateMovementInternalHandlerTests
    {
        private readonly CreateMovementInternalHandler handler;
        private readonly CreateMovementInternal request = 
            new CreateMovementInternal(new Guid("5E055311-2406-44E7-84FF-E35F4E004421"), 
                1, null, new DateTime(2015, 1, 1));
        private readonly ICapturedMovementFactory factory;
        private readonly IwsContext context;
        private readonly IMovementRepository movementRepository;

        public CreateMovementInternalHandlerTests()
        {
            context = new TestIwsContext();
            factory = A.Fake<ICapturedMovementFactory>();
            movementRepository = A.Fake<IMovementRepository>();

            handler = new CreateMovementInternalHandler(factory, movementRepository, context);
        }

        [Fact]
        public async Task CannotCreateMovement_ReturnsFalse()
        {
            A.CallTo(() => factory.Create(A<Guid>.Ignored, A<int>.Ignored, null, A<DateTime>.Ignored)).Throws(new MovementNumberException("Mike Merry"));

            var result = await handler.HandleAsync(request);

            Assert.False(result);
        }

        [Fact]
        public async Task CanCreateMovement_ReturnsTrue()
        {
            A.CallTo(() => factory.Create(A<Guid>.Ignored, A<int>.Ignored, null, A<DateTime>.Ignored))
                .Returns(new TestableMovement());

            var result = await handler.HandleAsync(request);

            Assert.True(result);
        } 
    }
}
