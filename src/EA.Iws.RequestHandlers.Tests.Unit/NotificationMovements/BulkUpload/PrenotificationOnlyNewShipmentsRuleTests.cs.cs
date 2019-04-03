namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.BulkPrenotification;
    using Core.Rules;
    using Domain.Movement;
    using FakeItEasy;
    using RequestHandlers.NotificationMovements.BulkPrenotification;
    using Xunit;

    public class PrenotificationOnlyNewShipmentsRuleTests
    {
        private readonly INotificationMovementsSummaryRepository repo;
        private readonly Guid notificationId = new Guid("DD1F019D-BD85-4A6F-89AB-328A7BD53CEA");

        private readonly PrenotificationInvalidShipmentNumberRule rule;

        public PrenotificationOnlyNewShipmentsRuleTests()
        {
            this.repo = A.Fake<INotificationMovementsSummaryRepository>();

            var maxShipments = 10;
            var currentShipments = 3;

            var movementSummary = NotificationMovementsSummary.Load(notificationId, string.Empty,
                Core.Shared.NotificationType.Disposal, maxShipments, currentShipments, 5, 3, 100, 10,
                Core.Shared.ShipmentQuantityUnits.Kilograms, Core.FinancialGuarantee.FinancialGuaranteeStatus.Approved,
                Core.Notification.UKCompetentAuthority.England, Core.NotificationAssessment.NotificationStatus.Consented,
                new Domain.ShipmentQuantity(1, Core.Shared.ShipmentQuantityUnits.Kilograms));

            A.CallTo(() => repo.GetById(notificationId)).Returns(movementSummary);

            rule = new PrenotificationInvalidShipmentNumberRule(repo);
        }

        [Fact]
        public async Task LastShipmentInUploadList_LessThanMaxShipments()
        {
            var movements = new List<PrenotificationMovement>()
            {
                new PrenotificationMovement()
                {
                    ShipmentNumber = 6
                }
            };

            var result = await rule.GetResult(movements.ToList(), notificationId);

            Assert.Equal(MessageLevel.Success, result.MessageLevel);
        }

        [Fact]
        public async Task LastShipmentInUploadList_GreaterThanMaxShipments()
        {
            var movements = new List<PrenotificationMovement>()
            {
                new PrenotificationMovement()
                {
                    ShipmentNumber = 11
                }
            };

            var result = await rule.GetResult(movements.ToList(), notificationId);

            Assert.Equal(MessageLevel.Error, result.MessageLevel);
        }
    }
}
