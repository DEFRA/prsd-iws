namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.BulkPrenotification;
    using Core.Rules;
    using Domain;
    using Domain.NotificationConsent;
    using FakeItEasy;
    using RequestHandlers.NotificationMovements.BulkPrenotification;
    using Xunit;

    public class PrenotificationShipmentBeyondConsentWindowRuleTests
    {
        private readonly INotificationConsentRepository repo;
        private readonly Guid notificationId = new Guid("DD1F019D-BD85-4A6F-89AB-328A7BD53CEA");

        private PrenotificationContentShipmentBeyondConsentedDateRule rule;

        public PrenotificationShipmentBeyondConsentWindowRuleTests()
        {
            repo = A.Fake<INotificationConsentRepository>();

            var dateRange = new DateRange(DateTime.Now.AddDays(1), DateTime.Now.AddDays(20));
            var consent = new Consent(notificationId, dateRange, "Test", Guid.NewGuid());
            A.CallTo(() => this.repo.GetByNotificationId(notificationId)).Returns(consent);

            rule = new PrenotificationContentShipmentBeyondConsentedDateRule(repo);
        }

        [Fact]
        public async Task NewShipmentsDateBeforeConsentedEndDate()
        {
            var movements = new List<PrenotificationMovement>()
            {
                new PrenotificationMovement()
                {
                    ShipmentNumber = 6,
                    ActualDateOfShipment = DateTime.Now.AddDays(2)
                }
            };

            var result = await rule.GetResult(movements.ToList(), notificationId);

            Assert.Equal(MessageLevel.Success.ToString(), result.MessageLevel.ToString());
        }

        [Fact]
        public async Task NewShipmentsDateAfterConsentedEndDate()
        {
            var movements = new List<PrenotificationMovement>()
            {
                new PrenotificationMovement()
                {
                    ShipmentNumber = 6,
                     ActualDateOfShipment = DateTime.Now.AddDays(2)
                },
                new PrenotificationMovement()
                {
                    ShipmentNumber = 7,
                     ActualDateOfShipment = DateTime.Now.AddDays(30)
                }
            };

            var result = await rule.GetResult(movements.ToList(), notificationId);

            Assert.Equal(MessageLevel.Error.ToString(), result.MessageLevel.ToString());
            Assert.Equal("Shipment 7: the actual date of shipment is beyond your permitted Consent Window and therefore you are not allowed to prenotify this shipment.", result.ErrorMessage);
        }

        [Fact]
        public async Task NewShipmentsDateBeforeConsentedEndDate_ShippingDatesEnteredInReverse()
        {
            var movements = new List<PrenotificationMovement>()
            {
                new PrenotificationMovement()
                {
                    ShipmentNumber = 7,
                     ActualDateOfShipment = DateTime.Now.AddDays(30)
                },
                new PrenotificationMovement()
                {
                    ShipmentNumber = 6,
                     ActualDateOfShipment = DateTime.Now.AddDays(2)
                }
            };

            var result = await rule.GetResult(movements.ToList(), notificationId);

            Assert.Equal(MessageLevel.Error.ToString(), result.MessageLevel.ToString());
            Assert.Equal("Shipment 7: the actual date of shipment is beyond your permitted Consent Window and therefore you are not allowed to prenotify this shipment.", result.ErrorMessage);
        }
    }
}
