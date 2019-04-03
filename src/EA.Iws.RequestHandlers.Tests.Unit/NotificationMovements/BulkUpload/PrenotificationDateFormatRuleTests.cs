namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.BulkPrenotification;
    using Core.Rules;
    using Prsd.Core;
    using RequestHandlers.NotificationMovements.BulkPrenotification;
    using Xunit;

    public class PrenotificationDateFormatRuleTests
    {
        private readonly PrenotificationDateFormatRule rule;

        public PrenotificationDateFormatRuleTests()
        {
            rule = new PrenotificationDateFormatRule();
        }

        [Fact]
        public async Task DateRule_Success()
        {
            var movements = new List<PrenotificationMovement>()
            {
                new PrenotificationMovement()
                {
                    ShipmentNumber = 1,
                    ActualDateOfShipment = SystemTime.UtcNow
                }
            };

            var result = await rule.GetResult(movements, Guid.NewGuid());

            Assert.Equal(PrenotificationContentRules.InvalidDateFormat, result.Rule);
            Assert.Equal(MessageLevel.Success, result.MessageLevel);
        }

        [Fact]
        public async Task DateRule_Error()
        {
            var movements = new List<PrenotificationMovement>()
            {
                new PrenotificationMovement()
                {
                    ShipmentNumber = 1,
                    ActualDateOfShipment = null
                }
            };

            var result = await rule.GetResult(movements, Guid.NewGuid());

            Assert.Equal(PrenotificationContentRules.InvalidDateFormat, result.Rule);
            Assert.Equal(MessageLevel.Error, result.MessageLevel);
        }
    }
}
