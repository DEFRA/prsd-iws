namespace EA.Iws.RequestHandlers.Tests.Unit.Producer
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.NotificationApplication;
    using FakeItEasy;
    using Helpers;
    using Producers;
    using Requests.Producers;
    using TestHelpers.Helpers;
    using Xunit;

    public class ProducerHandlerTests
    {
        private readonly IwsContext context;
        private readonly AddProducerToNotificationHandler addHandler;
        private readonly DeleteProducerForNotificationHandler deleteHandler;
        private readonly Guid countryId = new Guid("A62BD60E-9B81-4B8A-B59C-2B4579FF97E7");
        private readonly Guid notificationId = new Guid("5243D3E5-CA81-4A3E-B589-4D22D6676B28");
        private readonly Guid producerId = new Guid("51E43ADC-0A4A-4DBE-82F0-F60F0A7A16D7");
        private readonly NotificationApplication notification;

        public ProducerHandlerTests()
        {
            context = A.Fake<IwsContext>();
            var helper = new DbContextHelper();
            notification = new NotificationApplication(Guid.Empty, NotificationType.Recovery, UKCompetentAuthority.England, 0);
            EntityHelper.SetEntityId(notification, notificationId);

            A.CallTo(() => context.NotificationApplications).Returns(helper.GetAsyncEnabledDbSet(new[]
            {
                notification
            }));

            var countryWithData = CountryFactory.Create(countryId);
            A.CallTo(() => context.Countries).Returns(helper.GetAsyncEnabledDbSet(new[]
            {
                countryWithData
            }));

            addHandler = new AddProducerToNotificationHandler(context);
            deleteHandler = new DeleteProducerForNotificationHandler(context);
        }

        private async Task AddProducer()
        {
            var request = new AddProducerToNotification
            {
                NotificationId = notificationId,
                Address = SharedObjectFactory.GetAddressData(countryId),
                Business = SharedObjectFactory.GetBusinessInfoData(),
                Contact = SharedObjectFactory.GetContactData()
            };
            await addHandler.HandleAsync(request);
        }

        [Fact]
        public async Task CanAddProducer()
        {
            await AddProducer();
            Assert.True(notification.Producers.Any());
        }

        [Fact]
        public async Task CanRemoveProducer()
        {
            await AddProducer();
            Assert.True(notification.Producers.Any());
            EntityHelper.SetEntityId(notification.Producers.First(), producerId);

            var request = new DeleteProducerForNotification(producerId, notificationId);
            await deleteHandler.HandleAsync(request);

            Assert.Equal(0, notification.Producers.Count());
        }

        [Fact]
        public async Task CanRemoveProducerOtherThanSiteOfExport()
        {
            await AddProducer();
            await AddProducer();
            EntityHelper.SetEntityId(notification.Producers.Last(), producerId);
            int beforeProducersCount = notification.Producers.Count();

            var request = new DeleteProducerForNotification(producerId, notificationId);
            await deleteHandler.HandleAsync(request);

            Assert.True(notification.Producers.Count() == beforeProducersCount - 1);
        }

        [Fact]
        public async Task CannotRemoveSiteOfExportProducerForMoreThanOneProducers()
        {
            await AddProducer();
            await AddProducer();
            Assert.True(notification.Producers.Count() == 2);
            EntityHelper.SetEntityId(notification.Producers.First(), producerId);

            var request = new DeleteProducerForNotification(producerId, notificationId);
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await deleteHandler.HandleAsync(request));
        }
    }
}
