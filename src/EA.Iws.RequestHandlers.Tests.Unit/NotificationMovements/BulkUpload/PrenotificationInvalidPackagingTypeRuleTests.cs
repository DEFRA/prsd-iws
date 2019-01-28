namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Core.Rules;
    using Domain.NotificationApplication;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using RequestHandlers.NotificationMovements.BulkUpload;
    using Xunit;

    public class PrenotificationInvalidPackagingTypeRuleTests
    {
        private readonly IMediator mediator;
        private readonly INotificationApplicationRepository repo;
        private readonly Guid notificationId = new Guid("DD1F019D-BD85-4A6F-89AB-328A7BD53CEA");

        private PrenotificationContentInvalidPackagingTypeRule rule;

        private NotificationApplication notificationApplication;

        public PrenotificationInvalidPackagingTypeRuleTests()
        {
            this.mediator = A.Fake<IMediator>();
            this.repo = A.Fake<INotificationApplicationRepository>();

            notificationApplication = new NotificationApplication(Guid.NewGuid(), Core.Shared.NotificationType.Disposal, Core.Notification.UKCompetentAuthority.England, 1);

            List<PackagingInfo> packagingInfos = new List<PackagingInfo>()
            {
                PackagingInfo.CreatePackagingInfo(Core.PackagingType.PackagingType.Bag),
                PackagingInfo.CreatePackagingInfo(Core.PackagingType.PackagingType.Box)
            };

            notificationApplication.SetPackagingInfo(packagingInfos);

            A.CallTo(() => repo.GetById(notificationId)).Returns(notificationApplication);
        }

        [Fact]
        public async Task AllPackagingTypesAreAllowedOnNotification()
        {
            rule = new PrenotificationContentInvalidPackagingTypeRule(repo);

            List<PrenotificationMovement> movements = new List<PrenotificationMovement>()
            {
                new PrenotificationMovement()
                {
                   ShipmentNumber = 3,
                   PackagingTypes = new List<Core.PackagingType.PackagingType>()
                   {
                       Core.PackagingType.PackagingType.Bag,
                       Core.PackagingType.PackagingType.Box
                   }
                },
                new PrenotificationMovement()
                {
                   ShipmentNumber = 4,
                   PackagingTypes = new List<Core.PackagingType.PackagingType>()
                   {
                       Core.PackagingType.PackagingType.Bag
                   }
                }
            };

            var result = await rule.GetResult(movements, notificationId);

            Assert.Equal(MessageLevel.Success.ToString(), result.MessageLevel.ToString());
        }

        [Fact]
        public async Task SomePackagingTypesNotAllowedOnNotification()
        {
            rule = new PrenotificationContentInvalidPackagingTypeRule(repo);

            List<PrenotificationMovement> movements = new List<PrenotificationMovement>()
            {
                new PrenotificationMovement()
                {
                   ShipmentNumber = 3,
                   PackagingTypes = new List<Core.PackagingType.PackagingType>()
                   {
                       Core.PackagingType.PackagingType.Jerrican,
                       Core.PackagingType.PackagingType.Box
                   }
                },
                new PrenotificationMovement()
                {
                   ShipmentNumber = 4,
                   PackagingTypes = new List<Core.PackagingType.PackagingType>()
                   {
                       Core.PackagingType.PackagingType.Jerrican
                   }
                }
            };

            var result = await rule.GetResult(movements, notificationId);

            Assert.Equal(MessageLevel.Error.ToString(), result.MessageLevel.ToString());
            Assert.Equal("Shipment number 3, 4: the packaging type is not permitted on this notification.", result.ErrorMessage);
        }
    }
}
