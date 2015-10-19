namespace EA.Iws.RequestHandlers.Tests.Unit.MovementReceipt
{
    using System;
    using System.Threading.Tasks;
    using Core.Movement;
    using Core.Shared;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Shipment;
    using FakeItEasy;
    using Prsd.Core;
    using RequestHandlers.MovementReceipt;
    using Requests.MovementReceipt;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class GetMovementReceiptSummaryDataByMovementIdHandlerTests : TestBase
    {
        private readonly GetMovementReceiptSummaryDataByMovementIdHandler handler;
        private readonly Func<GetMovementReceiptSummaryDataByMovementId> getRequest = () => new GetMovementReceiptSummaryDataByMovementId(MovementId);
        private readonly TestableFinancialGuarantee financialGuarantee;
        private readonly TestableShipmentInfo shipmentInfo;
        private readonly IMovementRepository movementRepository;

        public GetMovementReceiptSummaryDataByMovementIdHandlerTests()
        {
            shipmentInfo = new TestableShipmentInfo();
            shipmentInfo.NotificationId = NotificationId;
            shipmentInfo.Quantity = 50;
            shipmentInfo.Units = ShipmentQuantityUnits.Kilograms;

            financialGuarantee = new TestableFinancialGuarantee
            {
                Id = FinancialGuaranteeId,
                NotificationApplicationId = NotificationId,
                ActiveLoadsPermitted = 5
            };

            NotificationApplication.NotificationNumber = "GB 00010 6103";

            Movement.Number = 1;
            Movement.Units = ShipmentQuantityUnits.Kilograms;
            Movement.Status = MovementStatus.Received;
            Movement.Receipt = new TestableMovementReceipt
            {
                Date = new DateTime(2015, 9, 1),
                Decision = Core.MovementReceipt.Decision.Accepted,
                Quantity = 5
            };

            Context.FinancialGuarantees.Add(financialGuarantee);

            var shipmentRepository = A.Fake<IShipmentInfoRepository>();
            A.CallTo(() => shipmentRepository.GetByNotificationId(NotificationId)).Returns(shipmentInfo);

            movementRepository = A.Fake<IMovementRepository>();
            A.CallTo(() => movementRepository.GetReceivedMovements(NotificationId)).Returns(new[] { Movement });
            A.CallTo(() => movementRepository.GetById(MovementId)).Returns(Movement);

            var notificationRepository = A.Fake<INotificationApplicationRepository>();
            A.CallTo(() => notificationRepository.GetByMovementId(MovementId)).Returns(NotificationApplication);

            handler = new GetMovementReceiptSummaryDataByMovementIdHandler(
                Context,
                new ActiveMovements(),
                new NotificationMovementsQuantity(movementRepository, shipmentRepository),
                shipmentRepository,
                movementRepository,
                notificationRepository);
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

            A.CallTo(() => movementRepository.GetAllMovements(NotificationId)).Returns(new[]
            {
                new TestableMovement
                {
                    NotificationId = NotificationId,
                    Date = new DateTime(2014, 9, 7),
                    Units = ShipmentQuantityUnits.Kilograms
                },
                new TestableMovement
                {
                    NotificationId = NotificationId,
                    Date = new DateTime(2014, 11, 15),
                    Units = ShipmentQuantityUnits.Kilograms
                },
                new TestableMovement
                {
                    NotificationId = NotificationId,
                    Date = new DateTime(2015, 2, 4),
                    Units = ShipmentQuantityUnits.Kilograms
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

            Assert.Equal(ShipmentQuantityUnits.Kilograms, result.DisplayUnit);
        }
    }
}
