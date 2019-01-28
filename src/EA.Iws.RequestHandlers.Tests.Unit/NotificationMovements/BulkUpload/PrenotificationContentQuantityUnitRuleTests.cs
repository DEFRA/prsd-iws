namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.BulkUpload
{
    using System.Collections.Generic;
    using Core.Movement.Bulk;
    using Core.Shared;
    using Domain.NotificationApplication.Shipment;
    using FakeItEasy;
    using RequestHandlers.NotificationMovements.BulkUpload;

    public class PrenotificationContentQuantityUnitRuleTests
    {
        private readonly PrenotificationContentQuantityUnitRule rule;

        public PrenotificationContentQuantityUnitRuleTests()
        {
            var shipmentRepo = A.Fake<IShipmentInfoRepository>();
            rule = new PrenotificationContentQuantityUnitRule(shipmentRepo);
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
