namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Core.Rules;
    using Core.Shared;
    using Domain;
    using Domain.NotificationApplication.Shipment;
    using FakeItEasy;
    using RequestHandlers.NotificationMovements.BulkUpload;
    using Xunit;

    public class PrenotificationContentQuantityUnitRuleTests
    {
        private readonly PrenotificationContentQuantityUnitRule rule;
        private readonly IShipmentInfoRepository shipmentInfoRepository;
        private readonly Guid notificationId;

        public PrenotificationContentQuantityUnitRuleTests()
        {
            shipmentInfoRepository = A.Fake<IShipmentInfoRepository>();
            rule = new PrenotificationContentQuantityUnitRule(shipmentInfoRepository);
            notificationId = Guid.NewGuid();
        }

        [Fact]
        public async Task GetResult_MatchingUnits_Success()
        {
            var notificationUnit = ShipmentQuantityUnits.Tonnes;

            A.CallTo(() => shipmentInfoRepository.GetByNotificationId(notificationId))
                .Returns(GetTestShipmentInfo(notificationUnit));

            var result = await rule.GetResult(GetTestData(notificationUnit), notificationId);

            Assert.Equal(BulkMovementContentRules.QuantityUnit, result.Rule);
            Assert.Equal(MessageLevel.Success, result.MessageLevel);
        }

        [Fact]
        public async Task GetResult_NotMatchingUnits_Error()
        {
            var notificationUnit = ShipmentQuantityUnits.Tonnes;
            var dataUnit = ShipmentQuantityUnits.Kilograms;

            A.CallTo(() => shipmentInfoRepository.GetByNotificationId(notificationId))
                .Returns(GetTestShipmentInfo(notificationUnit));

            var result = await rule.GetResult(GetTestData(dataUnit), notificationId);

            Assert.Equal(MessageLevel.Error, result.MessageLevel);
        }

        private ShipmentInfo GetTestShipmentInfo(ShipmentQuantityUnits unit)
        {
            var shipmentPeriod = new ShipmentPeriod(DateTime.Now, DateTime.Now.AddMonths(12), true); 
            var shipmentQuantity = new ShipmentQuantity(1.0m, unit);
            return new ShipmentInfo(notificationId, shipmentPeriod, 100, shipmentQuantity);
        }

        private List<PrenotificationMovement> GetTestData(ShipmentQuantityUnits unit)
        {
            return new List<PrenotificationMovement>()
            {
                new PrenotificationMovement()
                {
                    NotificationNumber = "GB 0001 001234",
                    ShipmentNumber = 1,
                    Unit = unit
                },
                new PrenotificationMovement()
                {
                    NotificationNumber = "GB 0001 001234",
                    ShipmentNumber = 2,
                    Unit = unit
                },
                new PrenotificationMovement()
                {
                    NotificationNumber = "GB 0001 001234",
                    ShipmentNumber = 3,
                    Unit = unit
                }
            };
        }
    }
}
