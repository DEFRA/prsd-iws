namespace EA.Iws.RequestHandlers.Tests.Unit.MovementReceipt
{
    using EA.Iws.Domain.MovementReceipt;
    using RequestHandlers.MovementReceipt;
    using System;
    using System.Threading.Tasks;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class MovementQuantityCalculatorTests : TestBase
    {
        private readonly MovementQuantityCalculator service;

        public MovementQuantityCalculatorTests()
        {
            Movement.Receipt = new TestableMovementReceipt
            {
                Date = new DateTime(2015, 9, 1),
                Decision = Core.MovementReceipt.Decision.Accepted,
                Quantity = 5
            };

            NotificationApplication.ShipmentInfo = new TestableShipmentInfo
            {
                Quantity = 20
            };

            Context.NotificationApplications.Add(NotificationApplication);
            Context.Movements.Add(Movement);

            service = new MovementQuantityCalculator(Context, new MovementReceiptService());
        }

        [Fact]
        public async Task ReturnsCorrectQuantityReceived()
        {
            Assert.Equal(5, await service.TotalQuantityReceivedAsync(NotificationId));
        }

        [Fact]
        public async Task ReturnsCorrectQuantityRemaining()
        {
            Assert.Equal(15, await service.TotalQuantityRemainingAsync(NotificationId));
        }
    }
}
