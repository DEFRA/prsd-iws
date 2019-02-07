namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.BulkReceiptRecovery
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Rules;
    using RequestHandlers.NotificationMovements.BulkReceiptRecovery;
    using Xunit;

    public class ReceiptRecoveryReceiptDateFormatRuleTests
    {
        private readonly ReceiptRecoveryReceiptDateFormatRule rule;
        private readonly Guid notificationId;

        public ReceiptRecoveryReceiptDateFormatRuleTests()
        {
            rule = new ReceiptRecoveryReceiptDateFormatRule();
            notificationId = Guid.NewGuid();
        }

        [Fact]
        public async Task GetResult_ReceiptDateFormat_Success()
        {
            var result = await rule.GetResult(GetTestData(false), notificationId);

            Assert.Equal(ReceiptRecoveryContentRules.ReceiptDateFormat, result.Rule);
            Assert.Equal(MessageLevel.Success, result.MessageLevel);
        }

        [Fact]
        public async Task GetResult_ReceiptDateFormat_Error()
        {
            var result = await rule.GetResult(GetTestData(true), notificationId);

            Assert.Equal(ReceiptRecoveryContentRules.ReceiptDateFormat, result.Rule);
            Assert.Equal(MessageLevel.Error, result.MessageLevel);
        }

        private List<ReceiptRecoveryMovement> GetTestData(bool invalidFormat)
        {
            DateTime? invalidDate = null;
            DateTime? validDate = DateTime.UtcNow;
            return new List<ReceiptRecoveryMovement>()
            {
                new ReceiptRecoveryMovement()
                {
                    ShipmentNumber = 1,
                    ReceivedDate = invalidFormat ? invalidDate : validDate,
                    MissingReceivedDate = false
                }
            };
        }
    }
}
