namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Core.Rules;
    using Domain;
    using Domain.NotificationApplication.Shipment;
    using FakeItEasy;
    using RequestHandlers.NotificationMovements.BulkUpload;
    using Xunit;

    public class PrenotificationShipmentBeyondConsentWindowRuleTests
    {
        private readonly IShipmentInfoRepository repo;
        private readonly Guid notificationId = new Guid("DD1F019D-BD85-4A6F-89AB-328A7BD53CEA");

        private PrenotificationContentShipmentBeyondConsentedDateRule rule;

        public PrenotificationShipmentBeyondConsentWindowRuleTests()
        {
            this.repo = A.Fake<IShipmentInfoRepository>();

            A.CallTo(() => this.repo.GetByNotificationId(notificationId)).Returns(new ShipmentInfo(this.notificationId, new ShipmentPeriod(DateTime.Now.AddDays(1), DateTime.Now.AddDays(20), false), 10, new ShipmentQuantity(10, Core.Shared.ShipmentQuantityUnits.Kilograms)));
            }

        [Fact]
        public async Task NewShipmentsDateBeforeConsentedEndDate()
        {
            rule = new PrenotificationContentShipmentBeyondConsentedDateRule(repo);

            List<PrenotificationMovement> movements = new List<PrenotificationMovement>()
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
            rule = new PrenotificationContentShipmentBeyondConsentedDateRule(repo);

            List<PrenotificationMovement> movements = new List<PrenotificationMovement>()
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
            rule = new PrenotificationContentShipmentBeyondConsentedDateRule(repo);

            List<PrenotificationMovement> movements = new List<PrenotificationMovement>()
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
