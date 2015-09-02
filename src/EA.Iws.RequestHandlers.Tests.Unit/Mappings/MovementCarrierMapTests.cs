namespace EA.Iws.RequestHandlers.Tests.Unit.Mappings
{
    using EA.Iws.TestHelpers.DomainFakes;
    using RequestHandlers.Mappings;
    using RequestHandlers.Mappings.Movement;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class MovementCarrierMapTests
    {
        private static readonly Guid AnyGuid = new Guid("F9725534-E887-48A0-8BD9-0C5855831B46");
        private readonly MovementCarrierMap map;
        private readonly TestableMovement movement;

        public MovementCarrierMapTests()
        {
            var context = new TestIwsContext();
            context.Countries.Add(TestableCountry.UnitedKingdom);

            var addressMap = new AddressMap(context);
            var businessMap = new BusinessInfoMap();
            var contactMap = new ContactMap();

            var carrierMap = new CarrierDataMap(addressMap, businessMap, contactMap);
            
            map = new MovementCarrierMap(carrierMap);
            movement = new TestableMovement();
        }

        [Fact]
        public void NullMovement_ReturnsNull()
        {
            var result = map.Map(null);

            Assert.Null(result);
        }

        [Fact]
        public void Movement_NoCarriers_ReturnsEmptyDictionary()
        {
            var result = map.Map(movement);

            Assert.Empty(result);
        }

        [Fact]
        public void Movement_HasCarriers_ReturnsNonEmptyDictionary()
        {
            var carrier = new TestableCarrier
            {
                Id = AnyGuid,
                Address = TestableAddress.SouthernHouse,
                Business = TestableBusiness.WasteSolutions,
                Contact = TestableContact.MikeMerry
            };

            var movementCarriers = new[] 
            {
                new TestableMovementCarrier
                {
                    Id = AnyGuid,
                    Carrier = carrier,
                    Order = 1
                }
            };

            movement.MovementCarriers = movementCarriers;

            var result = map.Map(movement);

            Assert.NotEmpty(result);
        }
    }
}
