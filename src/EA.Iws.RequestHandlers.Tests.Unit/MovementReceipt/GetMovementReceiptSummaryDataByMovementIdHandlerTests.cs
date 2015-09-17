namespace EA.Iws.RequestHandlers.Tests.Unit.MovementReceipt
{
    using EA.Iws.Domain.Movement;
    using EA.Iws.Domain.MovementReceipt;
    using EA.Iws.RequestHandlers.Movement;
    using EA.Iws.RequestHandlers.MovementReceipt;
    using EA.Iws.Requests.MovementReceipt;
    using EA.Iws.TestHelpers.DomainFakes;
    using EA.Prsd.Core;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class GetMovementReceiptSummaryDataByMovementIdHandlerTests : TestBase
    {
        private readonly GetMovementReceiptSummaryDataByMovementIdHandler handler;
        private readonly Func<GetMovementReceiptSummaryDataByMovementId> getRequest = () => new GetMovementReceiptSummaryDataByMovementId(MovementId);
        private readonly TestableFinancialGuarantee financialGuarantee;
        private readonly TestableShipmentInfo shipmentInfo;

        public GetMovementReceiptSummaryDataByMovementIdHandlerTests()
        {
            shipmentInfo = new TestableShipmentInfo();
            shipmentInfo.Quantity = 50;
            shipmentInfo.Units = Core.Shared.ShipmentQuantityUnits.Kilograms;

            financialGuarantee = new TestableFinancialGuarantee
            {
                Id = FinancialGuaranteeId,
                NotificationApplicationId = NotificationId,
                ActiveLoadsPermitted = 5
            };

            NotificationApplication.ShipmentInfo = shipmentInfo;
            NotificationApplication.NotificationNumber = "GB 00010 6103";

            Movement.Number = 1;
            Movement.Receipt = new TestableMovementReceipt
            {
                Date = new DateTime(2015, 9, 1),
                Decision = Core.MovementReceipt.Decision.Accepted,
                Quantity = 5
            };

            Context.Movements.Add(Movement);
            Context.NotificationApplications.Add(NotificationApplication);
            Context.FinancialGuarantees.Add(financialGuarantee);

            handler = new GetMovementReceiptSummaryDataByMovementIdHandler(
                Context, 
                new ActiveMovementCalculator(Context, new ActiveMovement()), 
                new MovementQuantityCalculator(Context, new MovementReceiptService()));
        }

        [Fact]
        public async Task MovementNotExists_Throws()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                handler.HandleAsync(new GetMovementReceiptSummaryDataByMovementId(Guid.Empty)));
        }

        [Fact]
        public async Task ReturnsCorrectNotificationNumber()
        {
            var result = await handler.HandleAsync(getRequest());

            Assert.Equal(NotificationApplication.NotificationNumber, result.NotificationNumber);
        }

        [Fact]
        public async Task ReturnsCorrectShipmentNumber()
        {
            var result = await handler.HandleAsync(getRequest());

            Assert.Equal(Movement.Number, result.ThisMovementNumber);
        }

        [Fact]
        public async Task ReturnsCorrectActiveLoadsPermitted()
        {
            var result = await handler.HandleAsync(getRequest());

            Assert.Equal(financialGuarantee.ActiveLoadsPermitted, result.ActiveLoadsPermitted);
        }

        [Fact]
        public async Task ReturnsCorrectCurrentActiveLoads_BasedOnNumberOfMovements_WithActualDateInPast()
        {
            SystemTime.Freeze(new DateTime(2015, 1, 1));

            Context.Movements.AddRange(new[]
            {
                new TestableMovement
                {
                    NotificationApplicationId = NotificationId,
                    Date = new DateTime(2014, 9, 7)
                },
                new TestableMovement
                {
                    NotificationApplicationId = NotificationId,
                    Date = new DateTime(2014, 11, 15)
                },
                new TestableMovement
                {
                    NotificationApplicationId = NotificationId,
                    Date = new DateTime(2015, 2, 4)
                }
            });

            var result = await handler.HandleAsync(getRequest());

            Assert.Equal(2, result.CurrentActiveLoads);

            SystemTime.Unfreeze();
        }

        [Fact]
        public async Task ReturnsCorrectQuantityReceivedSoFar_BasedOnMovementsWithCompleteReceipts()
        {
            var result = await handler.HandleAsync(getRequest());

            Assert.Equal(5, result.QuantitySoFar);
        }

        [Fact]
        public async Task ReturnsCorrectQuantityRemaining_BasedOnAmountFromNotification_LessQuantityRecieved()
        {
            var result = await handler.HandleAsync(getRequest());

            Assert.Equal(45, result.QuantityRemaining);
        }

        [Fact]
        public async Task ReturnsNotificationQuantityUnits_AsDisplayUnits()
        {
            var result = await handler.HandleAsync(getRequest());

            Assert.Equal(Core.Shared.ShipmentQuantityUnits.Kilograms, result.DisplayUnit);
        }
    }
}
