namespace EA.Iws.RequestHandlers.Tests.Unit.Movement
{
    using System;
    using System.Threading.Tasks;
    using Prsd.Core;
    using RequestHandlers.Mappings.Movement;
    using RequestHandlers.Movement;
    using Requests.Movement;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class GetMovementProgressInformationHandlerTests : TestBase
    {
        private readonly GetMovementProgressInformationHandler handler;
        private readonly Func<GetMovementProgressInformation> getRequest =
            () => new GetMovementProgressInformation(MovementId); 

        private readonly TestableMovement movement;
        private readonly TestableShipmentInfo shipmentInfo;
        private readonly TestableFinancialGuarantee financialGuarantee;

        public GetMovementProgressInformationHandlerTests()
        {
            shipmentInfo = new TestableShipmentInfo();

            financialGuarantee = new TestableFinancialGuarantee
            {
                Id = FinancialGuaranteeId,
                NotificationApplicationId = NotificationId
            };

            NotificationApplication.ShipmentInfo = shipmentInfo;
            NotificationApplication.NotificationNumber = "GB 001 00520";

            movement = new TestableMovement
            {
                Id = MovementId,
                NotificationApplicationId = NotificationId
            };

            Context.NotificationApplications.Add(NotificationApplication);
            Context.Movements.Add(movement);
            Context.FinancialGuarantees.Add(financialGuarantee);

            handler = new GetMovementProgressInformationHandler(Context, new MovementMap());
        }

        [Fact]
        public void RequestMapsCorrectInformation()
        {
            var request = getRequest();

            Assert.Equal(MovementId, request.MovementId);
        }

        [Fact]
        public async Task MovementDoesNotExist_Throws()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.HandleAsync(new GetMovementProgressInformation(Guid.Empty)));
        }

        [Fact]
        public async Task ReturnsCorrectNotificationNumber()
        {
            var result = await handler.HandleAsync(getRequest());

            Assert.Equal(NotificationApplication.NotificationNumber, result.NotificationNumber);
        }

        [Fact]
        public async Task ReturnsCorrectTotalShipments()
        {
            shipmentInfo.NumberOfShipments = 52;

            var result = await handler.HandleAsync(getRequest());

            Assert.Equal(52, result.TotalNumberOfMovements);
        }

        [Fact]
        public async Task ReturnsCorrectNumberForThisMovement()
        {
            movement.Number = 7;

            var result = await handler.HandleAsync(getRequest());

            Assert.Equal(7, result.ThisMovementNumber);
        }

        [Fact]
        public async Task ReturnsCorrectActiveLoadsPermitted()
        {
            financialGuarantee.ActiveLoadsPermitted = 7;

            var result = await handler.HandleAsync(getRequest());

            Assert.Equal(7, result.ActiveLoadsPermitted);
        }

        [Fact]
        public async Task ReturnsCurrentActiveLoads_BasedOnTotalNumberOfMovements_NotIncludingFutureMovements()
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
                    Date = new DateTime(2014, 6, 9)
                },
                new TestableMovement
                {
                    NotificationApplicationId = NotificationId,
                    Date = new DateTime(2015, 5, 3)
                }
            });

            var result = await handler.HandleAsync(getRequest());

            Assert.Equal(2, result.CurrentActiveLoads);

            SystemTime.Unfreeze();
        }
    }
}
