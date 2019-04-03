namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.BulkPrenotification;
    using Core.Rules;
    using RequestHandlers.NotificationMovements.BulkPrenotification;
    using Xunit;

    public class PrenotificationQuantityNumericTests
    {
        private readonly PrenotificationQuantityNumericRule rule;

        public PrenotificationQuantityNumericTests()
        {
            rule = new PrenotificationQuantityNumericRule();
        }

        [Fact]
        public async Task PrenotificationQuantityNumericRule_ValidData_Error()
        {
            var movements = new List<PrenotificationMovement>()
            {
                new PrenotificationMovement()
                {
                    ShipmentNumber = 1,
                    Quantity = null
                }
            };

            var result = await rule.GetResult(movements, Guid.NewGuid());

            Assert.Equal(PrenotificationContentRules.QuantityNumeric, result.Rule);
            Assert.Equal(MessageLevel.Error, result.MessageLevel);
        }

        [Fact]
        public async Task PrenotificationQuantityNumericRule_InvalidData_Error()
        {
            var movements = new List<PrenotificationMovement>()
            {
                new PrenotificationMovement()
                {
                    ShipmentNumber = 1,
                    Quantity = 10m
                }
            };

            var result = await rule.GetResult(movements, Guid.NewGuid());

            Assert.Equal(PrenotificationContentRules.QuantityNumeric, result.Rule);
            Assert.Equal(MessageLevel.Success, result.MessageLevel);
        }
    }
}
