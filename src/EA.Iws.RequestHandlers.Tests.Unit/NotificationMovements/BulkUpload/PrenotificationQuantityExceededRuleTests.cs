namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Core.Rules;
    using Domain.Movement;
    using FakeItEasy;
    using RequestHandlers.NotificationMovements.BulkUpload;
    using Xunit;

    public class PrenotificationQuantityExceededRuleTests
    {
        private readonly INotificationMovementsSummaryRepository repo;
        private readonly Guid notificationId = new Guid("DD1F019D-BD85-4A6F-89AB-328A7BD53CEA");

        private PrenotificationContentQuantityExceededRule rule;

        public PrenotificationQuantityExceededRuleTests()
        {
            this.repo = A.Fake<INotificationMovementsSummaryRepository>();

            int intendedQuantity = 100;
            int quantityReceived = 80;

            A.CallTo(() => repo.GetById(notificationId)).Returns(NotificationMovementsSummary.Load(notificationId, string.Empty, Core.Shared.NotificationType.Disposal, 10, 5, 10, 5, intendedQuantity, quantityReceived, Core.Shared.ShipmentQuantityUnits.Kilograms, Core.FinancialGuarantee.FinancialGuaranteeStatus.Approved, Core.Notification.UKCompetentAuthority.England, Core.NotificationAssessment.NotificationStatus.Consented, new Domain.ShipmentQuantity(1, Core.Shared.ShipmentQuantityUnits.Kilograms)));
        }

        [Fact]
        public async Task NewShipmentsQuantityLessThanQuantityAvailable()
        {
            rule = new PrenotificationContentQuantityExceededRule(repo);

            var movements = A.CollectionOfFake<PrenotificationMovement>(1);

            var result = await rule.GetResult(movements.ToList(), notificationId);

            Assert.Equal(MessageLevel.Success.ToString(), result.MessageLevel.ToString());
        }

        [Fact]
        public async Task NewShipmentsQuantityMoreThanQuantityAvailable()
        {
            rule = new PrenotificationContentQuantityExceededRule(repo);

            List<PrenotificationMovement> movements = new List<PrenotificationMovement>()
            {
                new PrenotificationMovement()
                {
                    ShipmentNumber = 6,
                    Quantity = 5
                },
                new PrenotificationMovement()
                {
                    ShipmentNumber = 7,
                    Quantity = 25
                }
            };

            var result = await rule.GetResult(movements.ToList(), notificationId);

            Assert.Equal(MessageLevel.Error.ToString(), result.MessageLevel.ToString());
            Assert.Equal("Shipment 7: the quantity of waste will exceed your permitted allowance and can't be prenotified.", result.ErrorMessage);
        }

        [Fact]
        public async Task NewShipmentsQuantityMoreThanQuantityAvailable_ShippingNumbersEnteredInReverse()
        {
            rule = new PrenotificationContentQuantityExceededRule(repo);

            List<PrenotificationMovement> movements = new List<PrenotificationMovement>()
            {
                new PrenotificationMovement()
                {
                    ShipmentNumber = 7,
                    Quantity = 25
                },
                new PrenotificationMovement()
                {
                    ShipmentNumber = 6,
                    Quantity = 5
                }
            };

            var result = await rule.GetResult(movements.ToList(), notificationId);

            Assert.Equal(MessageLevel.Error.ToString(), result.MessageLevel.ToString());
            Assert.Equal("Shipment 7: the quantity of waste will exceed your permitted allowance and can't be prenotified.", result.ErrorMessage);
        }
    }
}
