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

    public class GetMovementProgressInformationHandlerTests
    {
        private static readonly Guid MovementId = new Guid("CC8BA90F-D39C-4B24-A67F-20C96B60B232");
        private static readonly Guid NotificationId = new Guid("52D4CF08-5ED2-40FE-A39C-3FC565DF43ED");
        private static readonly Guid UserId = new Guid("D02B8E59-5D5D-44ED-B585-90A5E5F50C12");
        private static readonly Guid FinancialGuaranteeId = new Guid("B154EFBC-070B-4D22-AB91-60A18645CC05");

        private readonly GetMovementProgressInformationHandler handler;
        private readonly Func<GetMovementProgressInformation> getRequest =
            () => new GetMovementProgressInformation(MovementId); 

        private readonly TestIwsContext context;
        private readonly TestableMovement movement;
        private readonly TestableShipmentInfo shipmentInfo;
        private readonly TestableFinancialGuarantee financialGuarantee;
        private readonly TestableNotificationApplication notification;

        public GetMovementProgressInformationHandlerTests()
        {
            context = new TestIwsContext(new TestUserContext(UserId));

            shipmentInfo = new TestableShipmentInfo();

            financialGuarantee = new TestableFinancialGuarantee
            {
                Id = FinancialGuaranteeId,
                NotificationApplicationId = NotificationId
            };

            notification = new TestableNotificationApplication
            {
                Id = NotificationId,
                UserId = UserId,
                ShipmentInfo = shipmentInfo,
                NotificationNumber = "GB 001 00520"
            };

            movement = new TestableMovement
            {
                Id = MovementId
            };

            context.NotificationApplications.Add(notification);
            context.Movements.Add(movement);
            context.FinancialGuarantees.Add(financialGuarantee);

            handler = new GetMovementProgressInformationHandler(context, new MovementMap());
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

            Assert.Equal(notification.NotificationNumber, result.NotificationNumber);
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

            context.Movements.AddRange(new[]
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
