namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.BulkReceiptRecovery
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Rules;
    using RequestHandlers.NotificationMovements.BulkReceiptRecovery;
    using Xunit;

    public class ReceiptRecoveryQuantityNumericRuleTests
    {
        private readonly ReceiptRecoveryQuantityNumericRule rule;

        public ReceiptRecoveryQuantityNumericRuleTests()
        {
            rule = new ReceiptRecoveryQuantityNumericRule();
        }

        [Fact]
        public async Task ReceiptRecoveryQuantityNumericRule_InValidData_Error()
        {
            var movement = new List<ReceiptRecoveryMovement>()
            {
                new ReceiptRecoveryMovement()
                {
                    ShipmentNumber = 1,
                    Quantity = null
                }
            };

            var result = await rule.GetResult(movement, Guid.NewGuid());

            Assert.Equal(ReceiptRecoveryContentRules.QuantityNumeric, result.Rule);
            Assert.Equal(MessageLevel.Error, result.MessageLevel);
        }

        [Fact]
        public async Task ReceiptRecoveryQuantityNumericRule_ValidData_Success()
        {
            var movement = new List<ReceiptRecoveryMovement>()
            {
                new ReceiptRecoveryMovement()
                {
                    ShipmentNumber = 1,
                    Quantity = 10m
                }
            };

            var result = await rule.GetResult(movement, Guid.NewGuid());

            Assert.Equal(ReceiptRecoveryContentRules.QuantityNumeric, result.Rule);
            Assert.Equal(MessageLevel.Success, result.MessageLevel);
        }
    }
}
