namespace EA.Iws.RequestHandlers.Tests.Unit.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Admin.Search;
    using Cqrs.Tests.Unit.Helpers;
    using DataAccess;
    using Domain;
    using Domain.Notification;
    using FakeItEasy;
    using RequestHandlers.Admin.Search;
    using Requests.Admin;
    using TestHelpers.Helpers;
    using Xunit;

    public class GetBasicSearchResultsHandlerTests
    {
        private readonly DbContextHelper helper = new DbContextHelper();
        private string nonExistantSearchTerm;

        private System.Data.Entity.DbSet<NotificationApplication> GetNotificationApplications()
        {
            var applications = helper.GetAsyncEnabledDbSet(new[]
            {
                GetNotificationApplications("Exporter one", UKCompetentAuthority.Scotland, WasteType.CreateRdfWasteType(null)),
                GetNotificationApplications("GB 0001 000000", UKCompetentAuthority.England, WasteType.CreateRdfWasteType(null)),
                GetNotificationApplications("Exporter two", UKCompetentAuthority.Wales, WasteType.CreateSrfWasteType(null)),
                GetNotificationApplications("Exporter RDF", UKCompetentAuthority.Wales, WasteType.CreateWoodWasteType(null, null))
            });

            return applications;
        }

        private NotificationApplication GetNotificationApplications(string exporterName, UKCompetentAuthority competentAuthority, WasteType wasteType)
        {
            var notificationApplication = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery, competentAuthority, 0);

            var business = Business.CreateBusiness(exporterName, BusinessType.LimitedCompany, "irrelevant registration number", "irrelevant registration number");
            var address = ObjectFactory.CreateDefaultAddress();
            var contact = ObjectFactory.CreateEmptyContact();

            notificationApplication.SetExporter(business, address, contact);

            notificationApplication.SetWasteType(wasteType);

            return notificationApplication;
        }

        [Fact]
        public async Task SearchBy_NotificationNumber_AllDataMatches()
        {
            var results = await ResultsWhenSearchingFor("GB 0001 000000");

            Assert.Equal(1, results.Count);
            Assert.True(results.Any(r => r.NotificationNumber.Contains("GB 0001 000000")));
        }

        [Fact]
        public async Task SearchBy_NotificationNumber_NoDataMatch()
        {
            nonExistantSearchTerm = "GB 0001 005001";
            var results = await ResultsWhenSearchingFor(nonExistantSearchTerm);

            Assert.Equal(0, results.Count);
            Assert.True(!results.Any(r => r.NotificationNumber.Contains(nonExistantSearchTerm)));
        }

        [Fact]
        public async Task SearchBy_NotificationNumber_NotificationNumberWithoutSpace()
        {
            var results = await ResultsWhenSearchingFor("GB0001000000");

            Assert.Equal(1, results.Count);
        }

        [Fact]
        public async Task SearchBy_ExporterName()
        {
            var results = await ResultsWhenSearchingFor("Exporter one");

            Assert.Equal(1, results.Count);
        }

        [Fact]
        public async Task SearchBy_ExporterName_Multiples()
        {
            var results = await ResultsWhenSearchingFor("Exporter");

            Assert.Equal(3, results.Count);
        }

        [Fact]
        public async Task SearchBy_WasteType()
        {
            var results = await ResultsWhenSearchingFor("SRF");

            Assert.Equal(1, results.Count);
        }

        [Fact]
        public async Task Find_Nothing()
        {
            var results = await ResultsWhenSearchingFor("Exporter three");

            Assert.Equal(0, results.Count);
        }

        [Fact]
        public async Task SearchWithPartialNumber()
        {
            var results = await ResultsWhenSearchingFor("GB 0");

            Assert.Equal(GetNotificationApplications().Count(), results.Count);
        }

        private async Task<IList<BasicSearchResult>> ResultsWhenSearchingFor(string searchTerm)
        {
            var applications = GetNotificationApplications();

            var context = A.Fake<IwsContext>();
            A.CallTo(() => context.NotificationApplications).Returns(applications);

            var handler = new GetBasicSearchResultsHandler(context);
            return await handler.HandleAsync(new GetBasicSearchResults(searchTerm));
        }
    }
}