namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.BulkReceiptRecovery
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Rules;
    using RequestHandlers.NotificationMovements.BulkReceiptRecovery;
    using Xunit;

    public class ReceiptRecoveryDuplicateShipmentNumberRuleTests
    {
        private readonly ReceiptRecoveryDuplicateShipmentNumberRule rule;
        private readonly Guid notificationId;

        public ReceiptRecoveryDuplicateShipmentNumberRuleTests()
        {
            rule = new ReceiptRecoveryDuplicateShipmentNumberRule();
            notificationId = Guid.NewGuid();
        }

        [Fact]
        public async Task GetResult_DuplicateShipmentNumber_Success()
        {
            var result = await rule.GetResult(GetTestData(false), notificationId);

            Assert.Equal(ReceiptRecoveryContentRules.DuplicateShipmentNumber, result.Rule);
            Assert.Equal(MessageLevel.Success, result.MessageLevel);
        }

        [Fact]
        public async Task GetResult_DuplicateShipmentNumber_Error()
        {
            var result = await rule.GetResult(GetTestData(true), notificationId);

            Assert.Equal(ReceiptRecoveryContentRules.DuplicateShipmentNumber, result.Rule);
            Assert.Equal(MessageLevel.Error, result.MessageLevel);
        }

        private List<ReceiptRecoveryMovement> GetTestData(bool isDuplicated)
        {
            var duplicateShipmentNumber = 4;
            return new List<ReceiptRecoveryMovement>()
            {
                new ReceiptRecoveryMovement()
                {
                    ShipmentNumber = isDuplicated ? duplicateShipmentNumber : 1
                },
                new ReceiptRecoveryMovement()
                {
                    ShipmentNumber = isDuplicated ? duplicateShipmentNumber : 2
                },
                new ReceiptRecoveryMovement()
                {
                    ShipmentNumber = 3
                }
            };
        }
    }
}
