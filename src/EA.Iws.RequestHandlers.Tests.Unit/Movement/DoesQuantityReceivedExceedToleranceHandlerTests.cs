namespace EA.Iws.RequestHandlers.Tests.Unit.Movement
{
    using System;
    using System.Threading.Tasks;
    using Core.Movement;
    using Core.Shared;
    using Domain;
    using Domain.Movement;
    using FakeItEasy;
    using RequestHandlers.Movement.Receive;
    using Requests.Movement.Receive;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class DoesQuantityReceivedExceedToleranceHandlerTests
    {
        private readonly DoesQuantityReceivedExceedToleranceHandler handler;
        private readonly TestableMovementDetails details;

        public DoesQuantityReceivedExceedToleranceHandlerTests()
        {
            details = new TestableMovementDetails
            {
                ActualQuantity = new ShipmentQuantity(100, ShipmentQuantityUnits.Tonnes)
            };
            var movementDetailsRepository = A.Fake<IMovementDetailsRepository>();
            A.CallTo(() => movementDetailsRepository.GetByMovementId(A<Guid>.Ignored))
                .Returns(details);

            handler = new DoesQuantityReceivedExceedToleranceHandler(movementDetailsRepository);
        }

        [Theory]
        [InlineData(151, ShipmentQuantityUnits.Tonnes, QuantityReceivedTolerance.AboveTolerance)]
        [InlineData(151000, ShipmentQuantityUnits.Kilograms, QuantityReceivedTolerance.AboveTolerance)]
        [InlineData(49, ShipmentQuantityUnits.Tonnes, QuantityReceivedTolerance.BelowTolerance)]
        [InlineData(150, ShipmentQuantityUnits.Tonnes, QuantityReceivedTolerance.WithinTolerance)]
        public async Task ReturnsExpectedResult(int quantity, ShipmentQuantityUnits units, QuantityReceivedTolerance expected)
        {
            var result = await handler.HandleAsync(new DoesQuantityReceivedExceedTolerance(Guid.Empty, quantity, units));

            Assert.Equal(expected, result);
        }
    }
}
