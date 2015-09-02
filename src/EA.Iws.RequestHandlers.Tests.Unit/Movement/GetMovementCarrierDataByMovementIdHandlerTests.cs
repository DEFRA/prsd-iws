namespace EA.Iws.RequestHandlers.Tests.Unit.Movement
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using RequestHandlers.Mappings;
    using RequestHandlers.Mappings.Movement;
    using RequestHandlers.Movement;
    using Requests.Movement;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class GetMovementCarrierDataByMovementIdHandlerTests
    {
        private static readonly Guid MovementId = new Guid("687EE07E-D699-42FC-A9DB-F51D7B49F622");
        private static readonly Guid NotificationId = new Guid("33AE2679-DEE9-4B46-A4AF-3D4443A29FA1");
        private static readonly Guid Carrier1Id = new Guid("C437991A-4819-4AE6-9BA0-5833F623A8DE");
        private static readonly Guid Carrier2Id = new Guid("5CD5CA14-2E0A-43E3-992E-6AA24F3F9CB8");

        private readonly TestIwsContext context;
        private readonly GetMovementCarrierDataByMovementIdHandler handler;
        private readonly TestableMovement movement;
        private readonly TestableNotificationApplication notification;
        private readonly TestableCarrier carrier1;
        private readonly TestableCarrier carrier2;

        public GetMovementCarrierDataByMovementIdHandlerTests()
        {
            context = new TestIwsContext();

            context.Countries.Add(TestableCountry.UnitedKingdom);

            carrier1 = new TestableCarrier
            {
                Id = Carrier1Id,
                Address = TestableAddress.SouthernHouse,
                Business = TestableBusiness.WasteSolutions,
                Contact = TestableContact.MikeMerry
            };

            carrier2 = new TestableCarrier
            {
                Id = Carrier2Id,
                Address = TestableAddress.WitneyAddress,
                Business = TestableBusiness.LargeObjectHeap,
                Contact = TestableContact.BillyKnuckles
            };

            notification = new TestableNotificationApplication
            {
                Id = NotificationId,
                Carriers = new[] { carrier1, carrier2 }
            };

            movement = new TestableMovement
            {
                Id = MovementId,
                NotificationApplication = notification,
                NotificationApplicationId = NotificationId
            };

            context.NotificationApplications.Add(notification);
            context.Movements.Add(movement);

            var carrierDataMap = new CarrierDataMap(
                    new AddressMap(context),
                    new BusinessInfoMap(),
                    new ContactMap()); 

            var movementCarrierMap = new MovementCarrierMap(carrierDataMap);

            handler = new GetMovementCarrierDataByMovementIdHandler(
                context, 
                carrierDataMap,
                movementCarrierMap);
        }

        [Fact]
        public async Task MovementDoesNotExist_Throws()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.HandleAsync(new GetMovementCarrierDataByMovementId(Guid.Empty)));
        }

        [Fact]
        public async Task ReturnsNotificationCarriers()
        {
            var result = await handler.HandleAsync(new GetMovementCarrierDataByMovementId(MovementId));

            Assert.Equal(2, result.NotificationCarriers.Count);
            Assert.True(result.NotificationCarriers.Select(c => c.Id).Contains(Carrier1Id)
                && result.NotificationCarriers.Select(c => c.Id).Contains(Carrier2Id));
        }

        [Fact]
        public async Task ReturnsSelectedCarriersForMovement()
        {
            movement.MovementCarriers = new[] 
            {
                new TestableMovementCarrier
                {
                    Carrier = carrier1,
                    Order = 0
                }
            };

            var result = await handler.HandleAsync(new GetMovementCarrierDataByMovementId(MovementId));

            Assert.Equal(1, result.SelectedCarriers.Count);
            Assert.Equal(Carrier1Id, result.SelectedCarriers.Select(c => c.Value.Id).Single());
        }
    }
}
