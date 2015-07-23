namespace EA.Iws.RequestHandlers.Tests.Unit.Facility
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Cqrs.Tests.Unit.Helpers;
    using DataAccess;
    using Domain;
    using Domain.Notification;
    using Facilities;
    using FakeItEasy;
    using Requests.Facilities;
    using TestHelpers.Helpers;
    using Xunit;

    public class CopyFacilityFromImporterHandlerTests
    {
        private CopyFacilityFromImporterHandler handler;
        private NotificationApplication notification;
        private readonly Guid notificationId = new Guid("5243D3E5-CA81-4A3E-B589-4D22D6676B28");
        private CopyFacilityFromImporter message;

        public CopyFacilityFromImporterHandlerTests()
        {
            var context = A.Fake<IwsContext>();
            var helper = new DbContextHelper();
            notification = new NotificationApplication(Guid.Empty, NotificationType.Recovery, UKCompetentAuthority.England, 0);
            EntityHelper.SetEntityId(notification, notificationId);

            notification.SetImporter(ObjectFactory.CreateEmptyBusiness(), ObjectFactory.CreateDefaultAddress(), ObjectFactory.CreateEmptyContact());

            A.CallTo(() => context.NotificationApplications).Returns(helper.GetAsyncEnabledDbSet(new[]
            {
                notification
            }));

            message = new CopyFacilityFromImporter(notificationId);
            handler = new CopyFacilityFromImporterHandler(context);
        }

        [Fact]
        public async Task FacilityHasSameAddress()
        {
            await handler.HandleAsync(message);

            Assert.Equal(notification.Importer.Address, notification.Facilities.Single().Address);
        }

        [Fact]
        public async Task FacilityHasSameContact()
        {
            await handler.HandleAsync(message);

            Assert.Equal(notification.Importer.Contact, notification.Facilities.Single().Contact);
        }

        [Fact]
        public async Task FacilityHasSameBusinessName()
        {
            await handler.HandleAsync(message);

            Assert.Equal(notification.Importer.Business.Name, notification.Facilities.Single().Business.Name);
        }

        [Fact]
        public async Task FacilityHasSameBusinessType()
        {
            await handler.HandleAsync(message);

            Assert.Equal(notification.Importer.Business.Type, notification.Facilities.Single().Business.Type);
        }
    }
}