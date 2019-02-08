namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.BulkReceiptRecovery
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Rules;
    using RequestHandlers.NotificationMovements.BulkReceiptRecovery;
    using Xunit;

    public class ReceiptRecoveryRecoveryDateFormatRuleTests
    {
        private readonly ReceiptRecoveryRecoveryDateFormatRule rule;
        private readonly Guid notificationId;

        public ReceiptRecoveryRecoveryDateFormatRuleTests()
        {
            rule = new ReceiptRecoveryRecoveryDateFormatRule();
            notificationId = Guid.NewGuid();
        }

        [Fact]
        public async Task CorrectDate_Success()
        {
            var result = await rule.GetResult(GetTestData(false), notificationId);

            Assert.Equal(ReceiptRecoveryContentRules.RecoveryDateFormat, result.Rule);
            Assert.Equal(MessageLevel.Success, result.MessageLevel);
        }

        [Fact]
        public async Task IncorrectDate_Failure()
        {
            var result = await rule.GetResult(GetTestData(true), notificationId);

            Assert.Equal(ReceiptRecoveryContentRules.RecoveryDateFormat, result.Rule);
            Assert.Equal(MessageLevel.Error, result.MessageLevel);
        }

        private List<ReceiptRecoveryMovement> GetTestData(bool invalidDate)
        {
            DateTime? date = null;

            if (!invalidDate)
            {
                date = DateTime.Parse("20/02/2019");
            }

            return new List<ReceiptRecoveryMovement>()
            {
                new ReceiptRecoveryMovement()
                {
                    ShipmentNumber = 1,
                    RecoveredDisposedDate = date
                },
                new ReceiptRecoveryMovement()
                {
                    ShipmentNumber = 2,
                    RecoveredDisposedDate = DateTime.Parse("20/02/2019")
                }
            };
        }
    }
}
