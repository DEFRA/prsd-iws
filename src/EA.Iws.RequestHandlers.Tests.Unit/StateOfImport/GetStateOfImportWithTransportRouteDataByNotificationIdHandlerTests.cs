namespace EA.Iws.RequestHandlers.Tests.Unit.StateOfImport
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.NotificationApplication;
    using Domain.TransportRoute;
    using Mappings;
    using RequestHandlers.Mappings;
    using RequestHandlers.StateOfImport;
    using Requests.StateOfImport;
    using TestHelpers.Helpers;
    using Xunit;

    public class GetStateOfImportWithTransportRouteDataByNotificationIdHandlerTests
    {
        private readonly IwsContext context;
        private readonly GetStateOfImportWithTransportRouteDataByNotificationIdHandler handler;
        private readonly Guid stateOfImportCountryId = new Guid("012F9664-5286-433A-8628-AAE13FD1C2F5");
        private static readonly Guid NotificationNoStateOfImportId = new Guid("5243D3E5-CA81-4A3E-B589-4D22D6676B28");
        private static readonly Guid NotificationWithStateOfImportId = new Guid("6A28C99A-AF06-402D-BF6C-54A9A78D66A1");

        public GetStateOfImportWithTransportRouteDataByNotificationIdHandlerTests()
        {
            context = new TestIwsContext();

            var notificationNoStateOfImport = new NotificationApplication(TestIwsContext.UserId, NotificationType.Recovery,
                UKCompetentAuthority.England, 0);
            EntityHelper.SetEntityId(notificationNoStateOfImport, NotificationNoStateOfImportId);

            var country = CountryFactory.Create(stateOfImportCountryId);
            var stateOfImport = new StateOfImport(country, 
                CompetentAuthorityFactory.Create(Guid.Empty, country), 
                EntryOrExitPointFactory.Create(Guid.Empty, country));

            var notificationWithStateOfImport = new NotificationApplication(TestIwsContext.UserId, NotificationType.Recovery,
                UKCompetentAuthority.England, 0);
            EntityHelper.SetEntityId(notificationWithStateOfImport, NotificationWithStateOfImportId);

            ObjectInstantiator<NotificationApplication>.SetProperty(x => x.StateOfImport, stateOfImport, notificationWithStateOfImport);

            context.NotificationApplications.AddRange(new[]
            {
                notificationNoStateOfImport,
                notificationWithStateOfImport
            });

            context.Countries.AddRange(new[]
            {
                CountryFactory.Create(new Guid("93E82D5D-2135-4C6D-B66C-20D426E4BD12")),
                CountryFactory.Create(new Guid("FA626801-5659-4EA8-A64B-61916014663A")),
                country
            });

            context.CompetentAuthorities.Add(CompetentAuthorityFactory.Create(new Guid("03D04C92-594D-4E6E-B63E-9257211B263B"), country));

            context.EntryOrExitPoints.Add(EntryOrExitPointFactory.Create(new Guid("E098F49E-CD87-4EFE-AE29-FB87111045CF"), country));

            var countryMap = new CountryMap();
            var entryOrExitPointMap = new EntryOrExitPointMap();
            var competentAuthorityMap = new CompetentAuthorityMap();
            handler = new GetStateOfImportWithTransportRouteDataByNotificationIdHandler(context, 
                new StateOfImportMap(countryMap, competentAuthorityMap, entryOrExitPointMap),
                new StateOfExportMap(countryMap, competentAuthorityMap, entryOrExitPointMap),
                new TransitStateMap(countryMap, competentAuthorityMap, entryOrExitPointMap), 
                entryOrExitPointMap,
                countryMap, 
                competentAuthorityMap);
        }

        [Fact]
        public async Task Handler_NotificationDoesNotExist_Throws()
        {
            await
                Assert.ThrowsAsync<InvalidOperationException>(
                    () => handler.HandleAsync(new GetStateOfImportWithTransportRouteDataByNotificationId(Guid.Empty)));
        }

        [Fact]
        public async Task Handle_NotificationExistsNullStateOfImport_ReturnsNull()
        {
            var result = await handler.HandleAsync(new GetStateOfImportWithTransportRouteDataByNotificationId(NotificationNoStateOfImportId));

            Assert.Null(result.StateOfImport);
        }

        [Fact]
        public async Task Handle_NotificationExistsWithStateOfImport_ReturnsStateOfImport()
        {
            var result = await handler.HandleAsync(new GetStateOfImportWithTransportRouteDataByNotificationId(NotificationWithStateOfImportId));

            Assert.NotNull(result);
            Assert.Equal(stateOfImportCountryId, result.StateOfImport.Country.Id);
        }

        [Fact]
        public async Task Handle_NotificationExistsWithStateOfImport_ReturnsStateOfImportEntryPointsInCorrectCountry()
        {
            var result = await handler.HandleAsync(new GetStateOfImportWithTransportRouteDataByNotificationId(NotificationWithStateOfImportId));

            Assert.NotNull(result);
            Assert.True(result.EntryPoints.All(ep => ep.CountryId == stateOfImportCountryId));
        }

        [Fact]
        public async Task Handle_NotificationExistsWithStateOfImport_ReturnsStateOfImportCompetentAuthoritiesInCorrectCountry()
        {
            var result = await handler.HandleAsync(new GetStateOfImportWithTransportRouteDataByNotificationId(NotificationWithStateOfImportId));

            Assert.NotNull(result);
            Assert.True(result.CompetentAuthorities.All(ca => ca.CountryId == stateOfImportCountryId));
        }

        [Fact]
        public async Task Handle_ReturnsCountryData()
        {
            var result =
                await handler.HandleAsync(new GetStateOfImportWithTransportRouteDataByNotificationId(NotificationNoStateOfImportId));

            Assert.Equal(3, result.Countries.Length);
        }
    }
}
