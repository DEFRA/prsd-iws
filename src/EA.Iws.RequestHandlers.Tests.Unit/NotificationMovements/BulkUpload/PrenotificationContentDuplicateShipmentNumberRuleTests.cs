namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Core.Rules;
    using RequestHandlers.NotificationMovements.BulkUpload;
    using Xunit;

    public class PrenotificationContentDuplicateShipmentNumberRuleTests
    {
        private readonly PrenotificationContentDuplicateShipmentNumberRule rule;
        private readonly Guid notificationId;

        public PrenotificationContentDuplicateShipmentNumberRuleTests()
        {
            rule = new PrenotificationContentDuplicateShipmentNumberRule();
            notificationId = Guid.NewGuid();
        }

        [Fact]
        public async Task GetResult_DuplicateShipmentNumber_Success()
        {
            var result = await rule.GetResult(GetTestData(false), notificationId);

            Assert.Equal(BulkMovementContentRules.DuplicateShipmentNumber, result.Rule);
            Assert.Equal(MessageLevel.Success, result.MessageLevel);
        }

        [Fact]
        public async Task GetResult_DuplicateShipmentNumber_Error()
        {
            var result = await rule.GetResult(GetTestData(true), notificationId);

            Assert.Equal(BulkMovementContentRules.DuplicateShipmentNumber, result.Rule);
            Assert.Equal(MessageLevel.Error, result.MessageLevel);
        }

        private List<PrenotificationMovement> GetTestData(bool isDuplicated)
        {
            var duplicateShipmentNumber = 4;
            return new List<PrenotificationMovement>()
            {
                new PrenotificationMovement()
                {
                    ShipmentNumber = isDuplicated ? duplicateShipmentNumber : 1
                },
                new PrenotificationMovement()
                {
                    ShipmentNumber = isDuplicated ? duplicateShipmentNumber : 2
                },
                new PrenotificationMovement()
                {
                    ShipmentNumber = 3
                }
            };
        }
    }
}
