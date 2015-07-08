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
    using Helpers;
    using Requests.Facilities;
    using TestHelpers.Helpers;
    using Xunit;

    public class FacilityHandlerTests
    {
        private readonly IwsContext context;
        private readonly AddFacilityToNotificationHandler addHandler;
        private readonly DeleteFacilityForNotificationHandler deleteHandler;
        private readonly Guid countryId = new Guid("A62BD60E-9B81-4B8A-B59C-2B4579FF97E7");
        private readonly Guid notificationId = new Guid("5243D3E5-CA81-4A3E-B589-4D22D6676B28");
        private readonly Guid facilityId = new Guid("51E43ADC-0A4A-4DBE-82F0-F60F0A7A16D7");
        private readonly NotificationApplication notification;

        public FacilityHandlerTests()
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

            addHandler = new AddFacilityToNotificationHandler(context);
            deleteHandler = new DeleteFacilityForNotificationHandler(context);
        }

        private async Task AddFacility()
        {
            var request = new AddFacilityToNotification()
            {
                NotificationId = notificationId,
                Address = SharedObjectFactory.GetAddressData(countryId),
                Business = SharedObjectFactory.GetBusinessInfoData(),
                Contact = SharedObjectFactory.GetContactData()
            };
            await addHandler.HandleAsync(request);
        }

        [Fact]
        public async Task CanAddFacility()
        {
            await AddFacility();
            Assert.True(notification.Facilities.Any());
        }

        [Fact]
        public async Task CanRemoveFacility()
        {
            await AddFacility();
            Assert.True(notification.Facilities.Any());
            EntityHelper.SetEntityId(notification.Facilities.First(), facilityId);

            var request = new DeleteFacilityForNotification(notificationId, facilityId);
            await deleteHandler.HandleAsync(request);

            Assert.Equal(0, notification.Facilities.Count());
        }

        [Fact]
        public async Task CanRemoveFacilityOtherThanActualSiteOfTreatment()
        {
            await AddFacility();
            await AddFacility();
            EntityHelper.SetEntityId(notification.Facilities.Last(), facilityId);
            int beforeFacilitiesCount = notification.Facilities.Count();

            var request = new DeleteFacilityForNotification(notificationId, facilityId);
            await deleteHandler.HandleAsync(request);

            Assert.True(notification.Facilities.Count() == beforeFacilitiesCount - 1);
        }

        [Fact]
        public async Task CannotRemoveActualSiteOfTreatmentFacilityForMoreThanOneFacilities()
        {
            await AddFacility();
            await AddFacility();
            Assert.True(notification.Facilities.Count() == 2);
            EntityHelper.SetEntityId(notification.Facilities.First(), facilityId);

            var request = new DeleteFacilityForNotification(notificationId, facilityId);
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await deleteHandler.HandleAsync(request));
        }
    }
}
