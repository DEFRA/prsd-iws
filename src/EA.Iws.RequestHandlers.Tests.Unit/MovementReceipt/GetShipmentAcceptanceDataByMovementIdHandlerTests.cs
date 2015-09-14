namespace EA.Iws.RequestHandlers.Tests.Unit.MovementReceipt
{
    using System.Threading.Tasks;
    using Core.MovementReceipt;
    using RequestHandlers.MovementReceipt;
    using Requests.MovementReceipt;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class GetShipmentAcceptanceDataByMovementIdHandlerTests : TestBase
    {
        private static readonly string rejectionReason = "Rejection reason";

        [Fact]
        public async Task ReturnsDecisionAndRejectionReason()
        {
            var movement = new TestableMovement();
            movement.Id = MovementId;

            movement.Receipt = new TestableMovementReceipt
            {
                Decision = Decision.Rejected,
                RejectReason = rejectionReason
            };

            Context.Movements.Add(movement);

            var request = new GetMovementAcceptanceDataByMovementId(MovementId);
            var handler = new GetMovementAcceptanceDataByMovementIdHandler(Context);

            var result = await handler.HandleAsync(request);

            Assert.Equal(Decision.Rejected, result.Decision);
            Assert.Equal(rejectionReason, result.RejectionReason);
        }

        [Fact]
        public async Task ReturnsDecisionAndRejectionReasonWhenNull()
        {
            var movement = new TestableMovement();
            movement.Id = MovementId;

            movement.Receipt = new TestableMovementReceipt();

            Context.Movements.Add(movement);

            var request = new GetMovementAcceptanceDataByMovementId(MovementId);
            var handler = new GetMovementAcceptanceDataByMovementIdHandler(Context);

            var result = await handler.HandleAsync(request);

            Assert.Equal(null, result.Decision);
            Assert.Equal(null, result.RejectionReason);
        }
    }
}
