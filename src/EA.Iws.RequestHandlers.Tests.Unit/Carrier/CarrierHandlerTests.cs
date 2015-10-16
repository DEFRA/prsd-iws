namespace EA.Iws.RequestHandlers.Tests.Unit.Carrier
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Shared;
    using DataAccess;
    using Domain;
    using Domain.NotificationApplication;
    using Helpers;
    using RequestHandlers.Carrier;
    using Requests.Carriers;
    using TestHelpers.Helpers;
    using Xunit;

    public class CarrierHandlerTests
    {
        private readonly IwsContext context;
        private readonly AddCarrierToNotificationHandler addHandler;
        private readonly DeleteCarrierForNotificationHandler deleteHandler;
        private readonly Guid countryId = Guid.NewGuid();
        private readonly Guid notificationId = Guid.NewGuid();
        private readonly Guid carrierId = Guid.NewGuid();
        private readonly NotificationApplication notification;

        private int CarriersCount
        {
            get { return notification.Carriers.Count(); }
        }

        public CarrierHandlerTests()
        {
            context = new TestIwsContext();

            notification = new NotificationApplication(TestIwsContext.UserId, NotificationType.Recovery, UKCompetentAuthority.England, 0);
            EntityHelper.SetEntityId(notification, notificationId);

            context.NotificationApplications.Add(notification);

            var countryWithData = CountryFactory.Create(countryId);
            context.Countries.Add(countryWithData);

            addHandler = new AddCarrierToNotificationHandler(context);
            deleteHandler = new DeleteCarrierForNotificationHandler(context);
        }

        private async Task AddCarrier()
        {
            var request = new AddCarrierToNotification
            {
                NotificationId = notificationId,
                Address = SharedObjectFactory.GetAddressData(countryId),
                Business = SharedObjectFactory.GetBusinessInfoData(),
                Contact = SharedObjectFactory.GetContactData()
            };
            await addHandler.HandleAsync(request);
        }

        [Fact]
        public async Task CanAddCarrier()
        {
            await AddCarrier();
            Assert.Equal(CarriersCount, 1);
        }

        [Fact]
        public async Task CanRemoveCarrier()
        {
            await AddCarrier();
            Assert.True(notification.Carriers.Any());
            EntityHelper.SetEntityId(notification.Carriers.First(), carrierId);

            var request = new DeleteCarrierForNotification(notificationId, carrierId);
            await deleteHandler.HandleAsync(request);

            Assert.Equal(CarriersCount, 0);
        }

        [Fact]
        public async Task CanRemoveAllCarriers()
        {
            await AddCarrier();
            await AddCarrier();
            EntityHelper.SetEntityId(notification.Carriers.Last(), carrierId);
            int beforeCarriersCount = CarriersCount;

            var request = new DeleteCarrierForNotification(notificationId, carrierId);
            await deleteHandler.HandleAsync(request);

            Assert.Equal(CarriersCount, beforeCarriersCount - 1);
        }
    }
}
