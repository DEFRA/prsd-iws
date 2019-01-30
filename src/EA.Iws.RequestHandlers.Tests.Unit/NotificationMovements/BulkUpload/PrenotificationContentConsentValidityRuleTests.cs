namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Core.Rules;
    using Domain.NotificationConsent;
    using FakeItEasy;
    using RequestHandlers.NotificationMovements.BulkUpload;
    using Xunit;

    public class PrenotificationContentConsentValidityRuleTests
    {
        private readonly PrenotificationContentConsentValidityRule rule;
        private readonly INotificationConsentRepository notificationConsentRepository;
        private readonly Guid notificationId;
        private readonly Consent consent;

        public PrenotificationContentConsentValidityRuleTests()
        {
            notificationConsentRepository = A.Fake<INotificationConsentRepository>();
            rule = new PrenotificationContentConsentValidityRule(notificationConsentRepository);
            notificationId = Guid.NewGuid();
            consent = new Consent(Guid.NewGuid(), GetTestConsentDateRange(), string.Empty, Guid.NewGuid());
        }

        public static IEnumerable<object[]> CorrectData
        {
            get
            {
                return new[]
                {
                    new object[] { null },
                    new object[] { new DateTime(2019, 3, 1) },
                    new object[] { new DateTime(2019, 3, 2) },
                    new object[] { new DateTime(2019, 3, 30) },
                    new object[] { new DateTime(2019, 3, 31) }
                };
            }
        }

        public static IEnumerable<object[]> ErrorData
        {
            get
            {
                return new[]
                {
                    new object[] { new DateTime?(new DateTime(2019, 2, 28)) },
                    new object[] { new DateTime?(new DateTime(2019, 4, 1)) }
                };
            }
        }

        [Theory]
        [MemberData("CorrectData")]
        public async Task GetResult_ConsentRange_Success(DateTime? shipmentDate)
        {
            A.CallTo(() => notificationConsentRepository.GetByNotificationId(notificationId)).Returns(consent);

            var result = await rule.GetResult(GetTestData(shipmentDate), notificationId);

            Assert.Equal(BulkMovementContentRules.ConsentValidity, result.Rule);
            Assert.Equal(MessageLevel.Success, result.MessageLevel);
        }

        [Theory]
        [MemberData("ErrorData")]
        public async Task GetResult_ConsentRange_Error(DateTime? shipmentDate)
        {
            A.CallTo(() => notificationConsentRepository.GetByNotificationId(notificationId)).Returns(consent);

            var result = await rule.GetResult(GetTestData(shipmentDate), notificationId);

            Assert.Equal(BulkMovementContentRules.ConsentValidity, result.Rule);
            Assert.Equal(MessageLevel.Error, result.MessageLevel);
        }

        private Domain.DateRange GetTestConsentDateRange()
        {
            return new Domain.DateRange(new DateTime(2019, 3, 1), new DateTime(2019, 3, 31));
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
                    ActualDateOfShipment = shipmentDate,
                    ShipmentNumber = 2
                },
                new PrenotificationMovement()
                {
                    ActualDateOfShipment = shipmentDate,
                    ShipmentNumber = 3
                }
            };
        }
    }
}
