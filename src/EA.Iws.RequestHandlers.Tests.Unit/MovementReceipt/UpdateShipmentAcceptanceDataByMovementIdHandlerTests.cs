namespace EA.Iws.RequestHandlers.Tests.Unit.MovementReceipt
{
    using System;
    using System.Threading.Tasks;
    using Core.MovementReceipt;
    using RequestHandlers.MovementReceipt;
    using Requests.MovementReceipt;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class UpdateShipmentAcceptanceDataByMovementIdHandlerTests : TestBase
    {
        private readonly TestableMovement movement;
        private readonly UpdateMovementAcceptanceDataByMovementIdHandler handler;
        private readonly string reason = "Reason";

        public UpdateShipmentAcceptanceDataByMovementIdHandlerTests()
        {
            movement = new TestableMovement
            {
                Id = MovementId,
                Date = new DateTime(2015, 9, 1)
            };

            movement.Receipt = new TestableMovementReceipt();
            Context.Movements.Add(movement);

            handler = new UpdateMovementAcceptanceDataByMovementIdHandler(Context);
        }

        [Fact]
        public async Task DecisionAndReasonAreRecorded()
        {
            await handler.HandleAsync(new UpdateShipmentAcceptanceDataByMovementId(
                movement.Id, Decision.Rejected, reason));

            Assert.Equal(Decision.Rejected, movement.Receipt.Decision);
            Assert.Equal(reason, movement.Receipt.RejectReason);
        }

        [Fact]
        public async Task SaveChangesIsCalled()
        {
            await handler.HandleAsync(new UpdateShipmentAcceptanceDataByMovementId(
                    movement.Id, Decision.Rejected, reason));
            Assert.Equal(1, Context.SaveChangesCount);
        }

        [Fact]
        public async Task IfDecisionIsRejectedReasonIsNullThrows()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                handler.HandleAsync(new UpdateShipmentAcceptanceDataByMovementId(
                    movement.Id, Decision.Rejected, null)));
        }

        [Fact]
        public async Task IfDecisionIsAcceptedReasonIsSetToNull()
        {
            await handler.HandleAsync(new UpdateShipmentAcceptanceDataByMovementId(
                movement.Id, Decision.Accepted, reason));

            Assert.Equal(Decision.Accepted, movement.Receipt.Decision);
            Assert.Equal(null, movement.Receipt.RejectReason);
        }

        [Fact]
        public async Task ReasonGreaterThan200Throws()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.HandleAsync(new UpdateShipmentAcceptanceDataByMovementId(
                    movement.Id, Decision.Rejected, GetLongString())));
        }

        public string GetLongString()
        {
            return
                "This is a string that is longer than 200 characters so that the view model " +
                "will be invalid when it is entered in the reason for rejection field.  " +
                "That was one hundred and forty four so I am adding more text to increase the total count.";
        }
    }
}
