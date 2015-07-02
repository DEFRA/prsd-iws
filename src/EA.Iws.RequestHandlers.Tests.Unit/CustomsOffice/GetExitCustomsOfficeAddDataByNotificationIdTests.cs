namespace EA.Iws.RequestHandlers.Tests.Unit.CustomsOffice
{
    using System;
    using System.Threading.Tasks;
    using Cqrs.Tests.Unit.Helpers;
    using DataAccess;
    using Domain;
    using Domain.Notification;
    using Domain.TransportRoute;
    using FakeItEasy;
    using Mappings;
    using RequestHandlers.CustomsOffice;
    using Requests.CustomsOffice;
    using TestHelpers.Helpers;
    using Xunit;

    public class GetExitCustomsOfficeAddDataByNotificationIdTests
    {
        private static readonly Guid IgnoredGuid = Guid.Empty;
        private readonly GetExitCustomsOfficeAddDataByNotificationIdHandler handler;
        private readonly IwsContext context;
        private readonly DbContextHelper helper;
        private readonly ExitCustomsOffice exitCustomsOffice;
        private readonly Country country;
        private readonly NotificationApplication notification;

        public GetExitCustomsOfficeAddDataByNotificationIdTests()
        {
            context = A.Fake<IwsContext>();
            handler = new GetExitCustomsOfficeAddDataByNotificationIdHandler(context, 
                new CustomsOfficeExitMap(context,
                                            new CountryMap(), 
                                            new CustomsOfficeMap(new CountryMap())));

            helper = new DbContextHelper();

            country = CountryFactory.Create(new Guid("05C21C57-2F39-4A15-A09A-5F38CF139C05"));
            exitCustomsOffice = new ExitCustomsOffice("any name", "any address", country);

            notification = new NotificationApplication(IgnoredGuid, NotificationType.Recovery,
                UKCompetentAuthority.England, 500);
        }

        [Fact]
        public async Task Handle_NotificationDoesNotExist_Throws()
        {
            A.CallTo(() => context.NotificationApplications)
                .Returns(helper.GetAsyncEnabledDbSet(new NotificationApplication[] {}));

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => handler.HandleAsync(new GetExitCustomsOfficeAddDataByNotificationId(IgnoredGuid)));
        }

        [Fact]
        public async Task Handler_NotificationHasNoExitCustomsOffice_ReturnsStatusAndCountries()
        {
            A.CallTo(() => context.NotificationApplications)
                .Returns(helper.GetAsyncEnabledDbSet(new[] { notification }));

            A.CallTo(() => context.Countries).Returns(helper.GetAsyncEnabledDbSet(new[] { country }));

            var result = await handler.HandleAsync(new GetExitCustomsOfficeAddDataByNotificationId(IgnoredGuid));

            Assert.NotNull(result.Countries);
            Assert.Null(result.CustomsOfficeData);
            Assert.NotNull(result.CustomsOffices);
        }

        [Fact]
        public async Task Handler_NotificationHasExitCustomsOffice_ReturnsAllData()
        {
            ObjectInstantiator<NotificationApplication>.SetProperty(x => x.ExitCustomsOffice, exitCustomsOffice, notification);

            A.CallTo(() => context.NotificationApplications)
                .Returns(helper.GetAsyncEnabledDbSet(new[] { notification }));

            A.CallTo(() => context.Countries).Returns(helper.GetAsyncEnabledDbSet(new[] { country }));

            var result = await handler.HandleAsync(new GetExitCustomsOfficeAddDataByNotificationId(IgnoredGuid));

            Assert.Equal(1, result.Countries.Count);
            Assert.Equal(exitCustomsOffice.Name, result.CustomsOfficeData.Name);
            Assert.NotNull(result.CustomsOffices);
        }
    }
}
