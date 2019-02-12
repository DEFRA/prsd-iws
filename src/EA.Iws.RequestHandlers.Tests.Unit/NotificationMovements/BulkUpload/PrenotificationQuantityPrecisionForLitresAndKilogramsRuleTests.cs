namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.BulkPrenotification;
    using Core.Rules;
    using Core.Shared;
    using RequestHandlers.NotificationMovements.BulkPrenotification;
    using Xunit;

    public class PrenotificationQuantityPrecisionForLitresAndKilogramsRuleTests
    {
        private readonly PrenotificationQuantityPrecisionForLitresAndKilogramsRule rule;
        private readonly Guid notificationId;

        public PrenotificationQuantityPrecisionForLitresAndKilogramsRuleTests()
        {
            rule = new PrenotificationQuantityPrecisionForLitresAndKilogramsRule();
            notificationId = Guid.NewGuid();
        }

        [Fact]
        public async Task GetResult_Success()
        {
            var result = await rule.GetResult(GetTestData(true, false), notificationId);

            Assert.Equal(PrenotificationContentRules.QuantityPrecision, result.Rule);
            Assert.Equal(MessageLevel.Success, result.MessageLevel);
        }

        [Fact]
        public async Task GetResult_NotMatchingUnits_Success()
        {
            var result = await rule.GetResult(GetTestData(false, false), notificationId);

            Assert.Equal(MessageLevel.Success, result.MessageLevel);
        }

        [Fact]
        public async Task GetResult_ExceedPrecision_Error()
        {
            var result = await rule.GetResult(GetTestData(true, true), notificationId);

            Assert.Equal(MessageLevel.Error, result.MessageLevel);
            // Confirm both records failed
            result.ErrorMessage.Contains("1, 2");
        }

        private List<PrenotificationMovement> GetTestData(bool matchingUnit, bool exceedPrecision)
        {
            var unit1 = !matchingUnit ? ShipmentQuantityUnits.CubicMetres : ShipmentQuantityUnits.Kilograms;
            var unit2 = !matchingUnit ? ShipmentQuantityUnits.Tonnes : ShipmentQuantityUnits.Litres;
            var quantity = exceedPrecision ? 1.12m : 1.1m;

            return new List<PrenotificationMovement>()
            {
                new PrenotificationMovement()
                {
                    NotificationNumber = "GB 0001 001234",
                    ShipmentNumber = 1,
                    Unit = unit1,
                    Quantity = quantity
                },
                new PrenotificationMovement()
                {
                    NotificationNumber = "GB 0001 001234",
                    ShipmentNumber = 2,
                    Unit = unit2,
                    Quantity = quantity
                }
            };
        }
    }
}