namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.BulkReceiptRecovery
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Rules;
    using Core.Shared;
    using Domain;
    using Domain.NotificationApplication.Shipment;
    using RequestHandlers.NotificationMovements.BulkReceiptRecovery;
    using Xunit;

    public class ReceiptRecoveryQuantityPrecisionRuleTests
    {
        private readonly ReceiptRecoveryQuantityPrecisionRule rule;
        private readonly Guid notificationId;

        public ReceiptRecoveryQuantityPrecisionRuleTests()
        {
            rule = new ReceiptRecoveryQuantityPrecisionRule();
            notificationId = Guid.NewGuid();
        }

        [Fact]
        public async Task GetResult_MatchingUnits_Success()
        {
            var result = await rule.GetResult(GetTestData(ShipmentQuantityUnits.Tonnes), notificationId);

            Assert.Equal(ReceiptRecoveryContentRules.QuantityPrecision, result.Rule);
            Assert.Equal(MessageLevel.Success, result.MessageLevel);
        }

        [Fact]
        public async Task GetResult_NotMatchingUnits_Error()
        {
            var result = await rule.GetResult(GetTestData(ShipmentQuantityUnits.Kilograms), notificationId);

            Assert.Equal(MessageLevel.Error, result.MessageLevel);
        }

        private ShipmentInfo GetTestShipmentInfo(ShipmentQuantityUnits unit)
        {
            var shipmentPeriod = new ShipmentPeriod(DateTime.Now, DateTime.Now.AddMonths(12), true);
            var shipmentQuantity = new ShipmentQuantity(1.0m, unit);
            return new ShipmentInfo(notificationId, shipmentPeriod, 100, shipmentQuantity);
        }

        private List<ReceiptRecoveryMovement> GetTestData(ShipmentQuantityUnits unit)
        {
            return new List<ReceiptRecoveryMovement>()
            {
                new ReceiptRecoveryMovement()
                {
                    NotificationNumber = "GB 0001 001234",
                    ShipmentNumber = 1,
                    Unit = unit,
                    Quantity = 1.1m
                },
                new ReceiptRecoveryMovement()
                {
                    NotificationNumber = "GB 0001 001234",
                    ShipmentNumber = 2,
                    Unit = unit,
                    Quantity = 1.1234m
                },
                new ReceiptRecoveryMovement()
                {
                    NotificationNumber = "GB 0001 001234",
                    ShipmentNumber = 3,
                    Unit = unit,
                    Quantity = 10m
                }
            };
        }
    }
}