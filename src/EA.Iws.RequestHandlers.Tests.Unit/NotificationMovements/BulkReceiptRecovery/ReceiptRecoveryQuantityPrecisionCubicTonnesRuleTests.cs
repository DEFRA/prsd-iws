namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.BulkReceiptRecovery
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Rules;
    using Core.Shared;
    using RequestHandlers.NotificationMovements.BulkReceiptRecovery;
    using Xunit;

    public class ReceiptRecoveryQuantityPrecisionCubicTonnesRuleTests
    {
        private readonly ReceiptRecoveryQuantityPrecisionCubicTonnesRule rule;
        private readonly Guid notificationId;

        public ReceiptRecoveryQuantityPrecisionCubicTonnesRuleTests()
        {
            rule = new ReceiptRecoveryQuantityPrecisionCubicTonnesRule();
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
            var movements = new List<ReceiptRecoveryMovement>()
            {
                new ReceiptRecoveryMovement()
                {
                    NotificationNumber = "GB 0001 001234",
                    ShipmentNumber = 1,
                    Unit = ShipmentQuantityUnits.Tonnes,
                    Quantity = 1.12345m
                }
            };

            var result = await rule.GetResult(movements, notificationId);

            Assert.Equal(MessageLevel.Error, result.MessageLevel);
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