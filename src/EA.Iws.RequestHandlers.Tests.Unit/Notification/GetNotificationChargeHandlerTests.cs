namespace EA.Iws.RequestHandlers.Tests.Unit.Notification
{
    using System;
    using System.Threading.Tasks;
    using Core.Shared;
    using Cqrs.Tests.Unit.Helpers;
    using DataAccess;
    using Domain;
    using Domain.Notification;
    using FakeItEasy;
    using Prsd.Core;
    using RequestHandlers.Notification;
    using Requests.Notification;
    using TestHelpers.Helpers;
    using Xunit;
    using NotificationType = Domain.Notification.NotificationType;

    public class GetNotificationChargeHandlerTests : IDisposable
    {
        private readonly IwsContext context;
        private readonly GetNotificationChargeHandler handler;
        private readonly Guid notificationNoShipmentInfoId = new Guid("0A840C1D-E36F-424A-9063-0E8242717FB9");
        private readonly Guid notificationWithShipmentInfoId = new Guid("4A8D19E4-509C-4180-901E-3F941EBAB4E1");

        public GetNotificationChargeHandlerTests()
        {
            context = A.Fake<IwsContext>();
            var dbSetHelper = new DbContextHelper();

            var notifications = dbSetHelper.GetAsyncEnabledDbSet(new[]
            {
                NotificationNoShipmentInfo(),
                NotificationWithShipmentInfo()
            });

            var pricingStructures = dbSetHelper.GetAsyncEnabledDbSet(new[]
            {
                PricingStructureAll1000()
            });

            A.CallTo(() => context.PricingStructures).Returns(pricingStructures);
            A.CallTo(() => context.NotificationApplications).Returns(notifications);

            handler = new GetNotificationChargeHandler(new NotificationChargeCalculator(context));

            SystemTime.Freeze(new DateTime(2015, 7, 1));
        }

        public void Dispose()
        {
            SystemTime.Unfreeze();
        }

        private NotificationApplication NotificationNoShipmentInfo()
        {
            var notificationNoShipmentInfo = new NotificationApplication(Guid.Empty, NotificationType.Recovery,
                UKCompetentAuthority.England, 0);
            EntityHelper.SetEntityId(notificationNoShipmentInfo, notificationNoShipmentInfoId);
            return notificationNoShipmentInfo;
        }

        private NotificationApplication NotificationWithShipmentInfo()
        {
            var notificationWithShipmentInfo = new NotificationApplication(Guid.Empty, NotificationType.Recovery,
                UKCompetentAuthority.England, 0);
            EntityHelper.SetEntityId(notificationWithShipmentInfo, notificationWithShipmentInfoId);

            notificationWithShipmentInfo.SetShipmentInfo(new DateTime(2016, 1, 1), new DateTime(2016, 12, 31), 1, 1, ShipmentQuantityUnits.Kilogram);

            return notificationWithShipmentInfo;
        }

        private PricingStructure PricingStructureAll1000()
        {
            var pricingStructure = ObjectInstantiator<PricingStructure>.CreateNew();
            var activity = ObjectInstantiator<Activity>.CreateNew();
            var shipmentRange = ObjectInstantiator<ShipmentQuantityRange>.CreateNew();

            ObjectInstantiator<Activity>.SetProperty(x => x.TradeDirection, TradeDirection.Export, activity);
            ObjectInstantiator<Activity>.SetProperty(x => x.NotificationType, NotificationType.Recovery, activity);
            ObjectInstantiator<Activity>.SetProperty(x => x.IsInterim, false, activity);

            ObjectInstantiator<ShipmentQuantityRange>.SetProperty(x => x.RangeFrom, 1, shipmentRange);
            ObjectInstantiator<ShipmentQuantityRange>.SetProperty(x => x.RangeTo, 999999999, shipmentRange);

            ObjectInstantiator<PricingStructure>.SetProperty(x => x.CompetentAuthority, UKCompetentAuthority.England, pricingStructure);
            ObjectInstantiator<PricingStructure>.SetProperty(x => x.Activity, activity, pricingStructure);
            ObjectInstantiator<PricingStructure>.SetProperty(x => x.ShipmentQuantityRange, shipmentRange, pricingStructure);
            ObjectInstantiator<PricingStructure>.SetProperty(x => x.Price, 1000, pricingStructure);

            return pricingStructure;
        }

        [Fact]
        public async Task NotificationHasNoShipmentInfo_Returns0()
        {
            var request = new GetNotificationCharge(notificationNoShipmentInfoId);

            var result = await handler.HandleAsync(request);

            Assert.Equal(0, result);
        }

        [Fact]
        public async Task NotificationHasShipmentInfo_Returns1000()
        {
            var request = new GetNotificationCharge(notificationWithShipmentInfoId);

            var result = await handler.HandleAsync(request);

            Assert.Equal(1000, result);
        }
    }
}