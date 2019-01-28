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

    public class PrenotificationExcessiveShipmentsRuleTests
    {
        private readonly IMediator mediator;
        private readonly INotificationMovementsSummaryRepository repo;
        private readonly Guid notificationId = new Guid("DD1F019D-BD85-4A6F-89AB-328A7BD53CEA");

        private PrenotificationContentExcessiveShipmentsRule rule;

        public PrenotificationExcessiveShipmentsRuleTests()
        {
            this.mediator = A.Fake<IMediator>();
            this.repo = A.Fake<INotificationMovementsSummaryRepository>();

            int maxActiveLoads = 5;
            int currentActiveLoads = 3;

            A.CallTo(() => repo.GetById(notificationId)).Returns(NotificationMovementsSummary.Load(notificationId, string.Empty, Core.Shared.NotificationType.Disposal, 10, 5, maxActiveLoads, currentActiveLoads, 100, 10, Core.Shared.ShipmentQuantityUnits.Kilograms, Core.FinancialGuarantee.FinancialGuaranteeStatus.Approved, Core.Notification.UKCompetentAuthority.England, Core.NotificationAssessment.NotificationStatus.Consented, new Domain.ShipmentQuantity(1, Core.Shared.ShipmentQuantityUnits.Kilograms)));
        }

        [Fact]
        public async Task NewShipmentsLessThanActiveLoadsAvailable()
        {
            rule = new PrenotificationContentExcessiveShipmentsRule(repo);

            var movements = A.CollectionOfFake<PrenotificationMovement>(1);

            var result = await rule.GetResult(movements.ToList(), notificationId);

            Assert.Equal(MessageLevel.Success.ToString(), result.MessageLevel.ToString());
        }

        [Fact]
        public async Task ShipmentNumberLessThanExistingShipmentNumber()
        {
            rule = new PrenotificationContentExcessiveShipmentsRule(repo);

            var movements = A.CollectionOfFake<PrenotificationMovement>(3);

            var result = await rule.GetResult(movements.ToList(), notificationId);

            Assert.Equal(MessageLevel.Error.ToString(), result.MessageLevel.ToString());
            Assert.Equal("You can't create 3 shipments as there are only 2 active loads remaining.", result.ErrorMessage);
        }
    }
}
