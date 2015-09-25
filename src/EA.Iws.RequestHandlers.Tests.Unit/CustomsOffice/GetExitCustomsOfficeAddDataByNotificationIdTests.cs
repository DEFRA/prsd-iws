namespace EA.Iws.RequestHandlers.Tests.Unit.CustomsOffice
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.NotificationApplication;
    using Domain.TransportRoute;
    using FakeItEasy;
    using RequestHandlers.CustomsOffice;
    using RequestHandlers.Mappings;
    using Requests.CustomsOffice;
    using TestHelpers.Helpers;
    using Xunit;

    public class GetExitCustomsOfficeAddDataByNotificationIdTests
    {
        private static readonly Guid NotificationId = new Guid("FD7C6336-A825-4452-AF05-AF0A5D312327");
        private readonly GetExitCustomsOfficeAddDataByNotificationIdHandler handler;
        private readonly IwsContext context;
        private readonly ExitCustomsOffice exitCustomsOffice;
        private readonly Country country;
        private readonly NotificationApplication notification;
        private readonly TransportRoute transport;

        public GetExitCustomsOfficeAddDataByNotificationIdTests()
        {
            context = new TestIwsContext();
            var repository = A.Fake<ITransportRouteRepository>();

            country = CountryFactory.Create(new Guid("05C21C57-2F39-4A15-A09A-5F38CF139C05"));
            exitCustomsOffice = new ExitCustomsOffice("any name", "any address", country);

            notification = new NotificationApplication(TestIwsContext.UserId, NotificationType.Recovery,
                UKCompetentAuthority.England, 500);

            transport = new TransportRoute(NotificationId);

            EntityHelper.SetEntityId(notification, NotificationId);

            context.Countries.Add(country);
            context.NotificationApplications.Add(notification);
            context.TransportRoutes.Add(transport);

            A.CallTo(() => repository.GetByNotificationId(NotificationId)).Returns(transport);

            handler = new GetExitCustomsOfficeAddDataByNotificationIdHandler(repository,
                new CustomsOfficeExitMap(context,
                                            new CountryMap(),
                                            new CustomsOfficeMap(new CountryMap())));
        }

        [Fact]
        public async Task Handler_NotificationHasNoExitCustomsOffice_ReturnsStatusAndCountries()
        {
            var result = await handler.HandleAsync(new GetExitCustomsOfficeAddDataByNotificationId(NotificationId));

            Assert.NotNull(result.Countries);
            Assert.Null(result.CustomsOfficeData);
            Assert.NotNull(result.CustomsOffices);
        }

        [Fact]
        public async Task Handler_NotificationHasExitCustomsOffice_ReturnsAllData()
        {
            ObjectInstantiator<TransportRoute>.SetProperty(x => x.ExitCustomsOffice, exitCustomsOffice, transport);

            var result = await handler.HandleAsync(new GetExitCustomsOfficeAddDataByNotificationId(NotificationId));

            Assert.Equal(1, result.Countries.Count);
            Assert.Equal(exitCustomsOffice.Name, result.CustomsOfficeData.Name);
            Assert.NotNull(result.CustomsOffices);
        }
    }
}
