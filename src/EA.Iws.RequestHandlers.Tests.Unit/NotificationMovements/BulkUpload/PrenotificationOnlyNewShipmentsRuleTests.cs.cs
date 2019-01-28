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
    using Prsd.Core.Mediator;
    using RequestHandlers.NotificationMovements.BulkUpload;
    using Xunit;

    public class PrenotificationOnlyNewShipmentsRuleTests
    {
        private readonly IMediator mediator;
        private readonly INotificationMovementsSummaryRepository repo;
        private readonly Guid notificationId = new Guid("DD1F019D-BD85-4A6F-89AB-328A7BD53CEA");

        private PrenotificationContentInvalidShipmentNumberRule rule;

        public PrenotificationOnlyNewShipmentsRuleTests()
        {
            this.mediator = A.Fake<IMediator>();
            this.repo = A.Fake<INotificationMovementsSummaryRepository>();

            int maxShipments = 5;
            int currentShipments = 3;

            A.CallTo(() => repo.GetById(notificationId)).Returns(NotificationMovementsSummary.Load(notificationId, string.Empty, Core.Shared.NotificationType.Disposal, maxShipments, currentShipments, 5, 3, 100, 10, Core.Shared.ShipmentQuantityUnits.Kilograms, Core.FinancialGuarantee.FinancialGuaranteeStatus.Approved, Core.Notification.UKCompetentAuthority.England, Core.NotificationAssessment.NotificationStatus.Consented, new Domain.ShipmentQuantity(1, Core.Shared.ShipmentQuantityUnits.Kilograms)));
        }

        [Fact]
        public async Task LastShipmentInUploadList_LessThanMaxShipments()
        {
            rule = new PrenotificationContentInvalidShipmentNumberRule(repo);

            var movements = A.CollectionOfFake<PrenotificationMovement>(1);

            var result = await rule.GetResult(movements.ToList(), notificationId);

            Assert.Equal(MessageLevel.Success.ToString(), result.MessageLevel.ToString());
        }

        [Fact]
        public async Task LastShipmentInUploadList_GreaterThanMaxShipments()
        {
            rule = new PrenotificationContentInvalidShipmentNumberRule(repo);

            var movements = new List<PrenotificationMovement>()
            {
                new PrenotificationMovement()
                {
                    ShipmentNumber = 6
                }
            };

            var result = await rule.GetResult(movements.ToList(), notificationId);

            Assert.Equal(MessageLevel.Error.ToString(), result.MessageLevel.ToString());
            Assert.Equal("Shipment number 6: the shipment number is invalid - you've reached your shipment limit.", result.ErrorMessage);
        }
    }
}
