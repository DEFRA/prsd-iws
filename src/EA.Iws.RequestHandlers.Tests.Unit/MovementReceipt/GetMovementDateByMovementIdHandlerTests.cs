namespace EA.Iws.RequestHandlers.Tests.Unit.MovementReceipt
{
    using System;
    using System.Threading.Tasks;
    using Domain.Movement;
    using FakeItEasy;
    using RequestHandlers.Movement;
    using Requests.Movement;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class GetMovementDateByMovementIdHandlerTests : TestBase
    {
        private readonly GetMovementDateByMovementIdHandler handler;
        private readonly GetMovementDateByMovementId request;
        private readonly TestableMovement movement;
        private readonly IMovementRepository repository;

        private static readonly DateTime MovementDate = new DateTime(2015, 6, 1);
        private static readonly DateTime DateReceived = new DateTime(2015, 9, 1);

        public GetMovementDateByMovementIdHandlerTests()
        {
            movement = new TestableMovement
            {
                Id = MovementId,
                Date = MovementDate
            };

            repository = A.Fake<IMovementRepository>();
            A.CallTo(() => repository.GetById(MovementId)).Returns(movement);

            handler = new GetMovementDateByMovementIdHandler(repository);
            request = new GetMovementDateByMovementId(MovementId);
        }

        [Fact]
        public async Task ReturnsMovementDate()
        {
            var result = await handler.HandleAsync(request);

            Assert.Equal(MovementDate, result);
        }
    }
}