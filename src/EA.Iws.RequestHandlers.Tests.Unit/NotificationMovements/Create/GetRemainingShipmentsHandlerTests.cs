namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.Create
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.FinancialGuarantee;
    using Core.Movement;
    using Core.Shared;
    using Domain.FinancialGuarantee;
    using Domain.Movement;
    using Domain.NotificationApplication.Shipment;
    using FakeItEasy;
    using Prsd.Core;
    using RequestHandlers.NotificationMovements.Create;
    using Requests.NotificationMovements.Create;
    using TestHelpers.DomainFakes;
    using TestHelpers.Helpers;
    using Xunit;

    public class GetRemainingShipmentsHandlerTests : IDisposable
    {
        private readonly GetRemainingShipmentsHandler handler;
        private readonly IMovementRepository movementRepository;
        private readonly IShipmentInfoRepository shipmentRepository;
        private readonly IFinancialGuaranteeRepository financialGuaranteeRepository;

        private static readonly Guid NotificationId = new Guid("442191BE-6016-4545-B245-75E6941AA7BB");
        private readonly Guid userId = new Guid("E45663E5-1BD0-4AC3-999B-0E9975BE86FC");
        private static readonly DateTime Today = new DateTime(2019, 1, 1);
        
        private const int MaxNumberOfShipments = 30;
        private const int TotalMovements = 10;
        private const int ActiveMovements = 10;
        private const int ActiveLoadsPermitted = 20;

        public GetRemainingShipmentsHandlerTests()
        {
            SystemTime.Freeze(Today);

            movementRepository = A.Fake<IMovementRepository>();
            shipmentRepository = A.Fake<IShipmentInfoRepository>();
            financialGuaranteeRepository = A.Fake<IFinancialGuaranteeRepository>();

            var shipment = new TestableShipmentInfo
            {
                Id = Guid.NewGuid(),
                NotificationId = NotificationId,
                NumberOfShipments = MaxNumberOfShipments,
                Quantity = 10m,
                Units = ShipmentQuantityUnits.Tonnes
            };

            A.CallTo(() => shipmentRepository.GetByNotificationId(NotificationId)).Returns(shipment);
            A.CallTo(() => movementRepository.GetAllMovements(NotificationId)).Returns(GetShipments(TotalMovements, Today));
            A.CallTo(() => movementRepository.GetActiveMovements(NotificationId)).Returns(GetShipments(ActiveMovements, Today));

            handler = new GetRemainingShipmentsHandler(movementRepository, shipmentRepository, financialGuaranteeRepository);
        }

        [Fact]
        public async Task GetRemainingShipmentsHandler_GetsShipmentInfo()
        {
            var request = new GetRemainingShipments(NotificationId);

            await handler.HandleAsync(request);

            A.CallTo(() => shipmentRepository.GetByNotificationId(NotificationId))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task GetRemainingShipmentsHandler_GetsAllMovements()
        {
            var request = new GetRemainingShipments(NotificationId);

            await handler.HandleAsync(request);

            A.CallTo(() => movementRepository.GetAllMovements(NotificationId))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task GetRemainingShipmentsHandler_GetsActiveMovements()
        {
            var request = new GetRemainingShipments(NotificationId);

            await handler.HandleAsync(request);

            A.CallTo(() => movementRepository.GetActiveMovements(NotificationId))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task GetRemainingShipmentsHandler_GetsFinancialGuaranteeCollection()
        {
            var request = new GetRemainingShipments(NotificationId);

            await handler.HandleAsync(request);

            A.CallTo(() => financialGuaranteeRepository.GetByNotificationId(NotificationId))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task GetRemainingShipmentsHandler_GetFutureActiveMovementsNotCalled()
        {
            var request = new GetRemainingShipments(NotificationId);

            await handler.HandleAsync(request);

            A.CallTo(() => movementRepository.GetFutureActiveMovements(NotificationId))
                .MustNotHaveHappened();
        }

        [Fact]
        public async Task GetRemainingShipmentsHandler_GetFutureActiveMovements()
        {
            var request = new GetRemainingShipments(NotificationId, Today);

            await handler.HandleAsync(request);

            A.CallTo(() => movementRepository.GetFutureActiveMovements(NotificationId))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task GetRemainingShipmentsHandler_ApprovedFG_ReturnsResponse()
        {
            var request = new GetRemainingShipments(NotificationId);

            A.CallTo(() => financialGuaranteeRepository.GetByNotificationId(NotificationId))
                .Returns(GetFinancialGuarantee(FinancialGuaranteeStatus.Approved));

            var response = await handler.HandleAsync(request);

            Assert.Equal(ActiveLoadsPermitted, response.ActiveLoadsPermitted);
            Assert.Equal(ActiveLoadsPermitted - ActiveMovements, response.ActiveLoadsRemaining);
            Assert.Equal(MaxNumberOfShipments - TotalMovements, response.ShipmentsRemaining);
        }

        [Fact]
        public async Task GetRemainingShipmentsHandler_NoApprovedFG_ReturnsZeroActiveLoadsPermitted()
        {
            var request = new GetRemainingShipments(NotificationId);
            var noApprovedFGActiveLoads = 0;

            A.CallTo(() => financialGuaranteeRepository.GetByNotificationId(NotificationId))
                .Returns(GetFinancialGuarantee(FinancialGuaranteeStatus.ApplicationReceived));

            var response = await handler.HandleAsync(request);

            Assert.Equal(noApprovedFGActiveLoads, response.ActiveLoadsPermitted);
            Assert.Equal(noApprovedFGActiveLoads - ActiveMovements, response.ActiveLoadsRemaining);
        }

        [Fact]
        public async Task GetRemainingShipmentsHandler_ApprovedFG_ReturnsFutureShipmentByDate()
        {
            var futureShipmentsDate = Today.AddDays(10);
            var futureShipmentsTotal = ActiveLoadsPermitted - 5;

            var request = new GetRemainingShipments(NotificationId, futureShipmentsDate);

            A.CallTo(() => financialGuaranteeRepository.GetByNotificationId(NotificationId))
                .Returns(GetFinancialGuarantee(FinancialGuaranteeStatus.Approved));

            A.CallTo(() => movementRepository.GetFutureActiveMovements(NotificationId))
                .Returns(GetShipments(futureShipmentsTotal, futureShipmentsDate));

            var response = await handler.HandleAsync(request);

            Assert.Equal(ActiveLoadsPermitted - futureShipmentsTotal, response.ShipmentsRemainingByDate);
        }

        [Fact]
        public async Task GetRemainingShipmentsHandler_NoApprovedFG_ReturnsFutureShipmentByDate()
        {
            var futureShipmentsDate = Today.AddDays(10);
            var futureShipmentsTotal = ActiveLoadsPermitted - 5;
            var noApprovedFGActiveLoads = 0;

            var request = new GetRemainingShipments(NotificationId, futureShipmentsDate);

            A.CallTo(() => financialGuaranteeRepository.GetByNotificationId(NotificationId))
                .Returns(GetFinancialGuarantee(FinancialGuaranteeStatus.Refused));

            A.CallTo(() => movementRepository.GetFutureActiveMovements(NotificationId))
                .Returns(GetShipments(futureShipmentsTotal, futureShipmentsDate));

            var response = await handler.HandleAsync(request);

            Assert.Equal(noApprovedFGActiveLoads - futureShipmentsTotal, response.ShipmentsRemainingByDate);
        }

        public void Dispose()
        {
            SystemTime.Unfreeze();
        }

        private static IEnumerable<TestableMovement> GetShipments(int n, DateTime date)
        {
            var movements = new List<TestableMovement>();

            for (var i = 0; i < n; i++)
            {
                movements.Add(new TestableMovement()
                {
                    NotificationId = NotificationId,
                    Status = MovementStatus.Submitted,
                    Date = date
                });
            }

            return movements;
        }

        private static FinancialGuaranteeCollection GetFinancialGuarantee(FinancialGuaranteeStatus status)
        {
            var collection = new FinancialGuaranteeCollection(NotificationId);

            var fg = collection.AddFinancialGuarantee(Today);
            ObjectInstantiator<FinancialGuarantee>.SetProperty(f => f.ActiveLoadsPermitted, ActiveLoadsPermitted, fg);
            ObjectInstantiator<FinancialGuarantee>.SetProperty(f => f.Status, status, fg);

            return collection;
        }
    }
}
