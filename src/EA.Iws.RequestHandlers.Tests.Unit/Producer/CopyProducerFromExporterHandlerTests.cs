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

    public class CopyProducerFromExporterHandlerTests
    {
        private CopyProducerFromExporterHandler handler;
        private NotificationApplication notification;
        private readonly Guid notificationId = new Guid("5243D3E5-CA81-4A3E-B589-4D22D6676B28");
        private CopyProducerFromExporter message;

        public CopyProducerFromExporterHandlerTests()
        {
            var context = A.Fake<IwsContext>();
            var helper = new DbContextHelper();
            notification = new NotificationApplication(Guid.Empty, NotificationType.Recovery, UKCompetentAuthority.England, 0);
            EntityHelper.SetEntityId(notification, notificationId);

            notification.SetExporter(ObjectFactory.CreateEmptyBusiness(), ObjectFactory.CreateDefaultAddress(), ObjectFactory.CreateEmptyContact());

            A.CallTo(() => context.NotificationApplications).Returns(helper.GetAsyncEnabledDbSet(new[]
            {
                notification
            }));

            message = new CopyProducerFromExporter(notificationId);
            handler = new CopyProducerFromExporterHandler(context);
        }

        [Fact]
        public async Task ProducerHasSameAddress()
        {
            await handler.HandleAsync(message);

            Assert.Equal(notification.Exporter.Address, notification.Producers.Single().Address);
        }

        [Fact]
        public async Task ProducerHasSameContact()
        {
            await handler.HandleAsync(message);

            Assert.Equal(notification.Exporter.Contact, notification.Producers.Single().Contact);
        }

        [Fact]
        public async Task ProducerHasSameBusinessName()
        {
            await handler.HandleAsync(message);

            Assert.Equal(notification.Exporter.Business.Name, notification.Producers.Single().Business.Name);
        }

        [Fact]
        public async Task ProducerHasSameBusinessType()
        {
            await handler.HandleAsync(message);

            Assert.Equal(notification.Exporter.Business.Type, notification.Producers.Single().Business.Type);
        }
    }
}