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

    public class PrenotificationContentDateHistoricRuleTests
    {
        private readonly PrenotificationContentDateHistoricRule rule;
        private readonly Guid notificationId;
        private static DateTime dateTimeNow;

        public PrenotificationContentDateHistoricRuleTests()
        {
            rule = new PrenotificationContentDateHistoricRule();
            notificationId = Guid.NewGuid();

            // Add a minute to the current time so that when processing it is still in the future
            dateTimeNow = DateTime.UtcNow.AddMinutes(1);
        }

        public static IEnumerable<object[]> CorrectData
        {
            get
            {
                return new[]
                {
                    new object[] { null },
                    new object[] { new DateTime(2030, 1, 1) }
                };
            }
        }

        public static IEnumerable<object[]> ErrorData
        {
            get
            {
                return new[]
                {
                    new object[] { new DateTime(2015, 1, 1) }
                };
            }
        }

        [Theory]
        [MemberData("CorrectData")]
        public async Task GetResult_DateHistoric_Success(DateTime? shipmentDate)
        {
            var result = await rule.GetResult(GetTestData(shipmentDate), notificationId);

            Assert.Equal(BulkMovementContentRules.HistoricDate, result.Rule);
            Assert.Equal(MessageLevel.Success, result.MessageLevel);
        }

        [Theory]
        [MemberData("ErrorData")]
        public async Task GetResult_DateHistoric_Error(DateTime? shipmentDate)
        {
            var result = await rule.GetResult(GetTestData(shipmentDate), notificationId);

            Assert.Equal(BulkMovementContentRules.HistoricDate, result.Rule);
            Assert.Equal(MessageLevel.Error, result.MessageLevel);
        }

        private List<PrenotificationMovement> GetTestData(DateTime? shipmentDate)
        {
            return new List<PrenotificationMovement>()
            {
                new PrenotificationMovement()
                {
                    ActualDateOfShipment = shipmentDate,
                    ShipmentNumber = 1
                },
                new PrenotificationMovement()
                {
                    ActualDateOfShipment = dateTimeNow,
                    ShipmentNumber = 2
                },
                new PrenotificationMovement()
                {
                    ActualDateOfShipment = dateTimeNow,
                    ShipmentNumber = 3
                }
            };
        }
    }
}
