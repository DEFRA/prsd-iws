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

    public class PrenotificationDateFutureRuleTests
    {
        private readonly PrenotificationDateFutureRule rule;
        private readonly Guid notificationId;
        private const int MaxDays = 30;

        public PrenotificationDateFutureRuleTests()
        {
            rule = new PrenotificationDateFutureRule();

            notificationId = Guid.NewGuid();
        }

        [Fact]
        public async Task LessThanMaxDays_Success()
        {
            var movements = new List<PrenotificationMovement>()
            {
                new PrenotificationMovement()
                {
                    ShipmentNumber = 1,
                    ActualDateOfShipment = SystemTime.UtcNow.AddDays(MaxDays - 10)
                },
                new PrenotificationMovement()
                {
                    ShipmentNumber = 2,
                    ActualDateOfShipment = SystemTime.UtcNow.AddDays(MaxDays - 20)
                }
            };

            var result = await rule.GetResult(movements, notificationId);

            Assert.Equal(PrenotificationContentRules.FutureDate, result.Rule);
            Assert.Equal(MessageLevel.Success, result.MessageLevel);
        }

        [Fact]
        public async Task MoreThanMaxDays_Error()
        {
            var movements = new List<PrenotificationMovement>()
            {
                new PrenotificationMovement()
                {
                    ShipmentNumber = 1,
                    ActualDateOfShipment = SystemTime.UtcNow.AddDays(MaxDays + 1)
                },
                new PrenotificationMovement()
                {
                    ShipmentNumber = 2,
                    ActualDateOfShipment = SystemTime.UtcNow.AddDays(MaxDays + 10)
                }
            };

            var result = await rule.GetResult(movements, notificationId);

            Assert.Equal(PrenotificationContentRules.FutureDate, result.Rule);
            Assert.Equal(MessageLevel.Error, result.MessageLevel);
        }
    }
}
