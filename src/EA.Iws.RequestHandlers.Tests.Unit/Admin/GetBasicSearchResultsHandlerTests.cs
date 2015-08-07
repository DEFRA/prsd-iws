namespace EA.Iws.RequestHandlers.Tests.Unit.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Admin.Search;
    using Core.NotificationAssessment;
    using DataAccess;
    using Domain;
    using Domain.NotificationApplication;
    using FakeItEasy;
    using Helpers;
    using Prsd.Core.Helpers;
    using RequestHandlers.Admin.Search;
    using Requests.Admin;
    using TestHelpers.Helpers;
    using Xunit;

    public class GetBasicSearchResultsHandlerTests
    {
        private readonly DbContextHelper helper = new DbContextHelper();
        private string nonExistantSearchTerm;
        private readonly Guid notification1 = new Guid("1A2A6255-A0B1-48B2-B248-D606B1BFB1DA");
        private readonly Guid notification2 = new Guid("879909D1-46BB-4396-8497-950EA4D29AB8");
        private readonly Guid notification3 = new Guid("268F7CBA-3146-4F88-9F07-A06B1BD36936");
        private readonly Guid notification4 = new Guid("D961B020-971E-44D4-8F27-C4B70791CAAA");
        private readonly Guid notification5 = new Guid("5C1DB181-2D02-42F5-A8BD-4EFA293007EC");
        private readonly GetBasicSearchResultsHandler handler;

        public GetBasicSearchResultsHandlerTests()
        {
            var applications = GetNotificationApplications();
            var assessments = GetNotificationAssessments();

            var context = A.Fake<IwsContext>();
            A.CallTo(() => context.NotificationApplications).Returns(applications);
            A.CallTo(() => context.NotificationAssessments).Returns(assessments);

            handler = new GetBasicSearchResultsHandler(context);
        }

        private System.Data.Entity.DbSet<NotificationApplication> GetNotificationApplications()
        {
            return helper.GetAsyncEnabledDbSet(new[]
            {
                CreateNotificationApplication(notification1, "Exporter one", UKCompetentAuthority.Scotland, WasteType.CreateRdfWasteType(null)),
                CreateNotificationApplication(notification2, "GB 0001 000000", UKCompetentAuthority.England, WasteType.CreateRdfWasteType(null)),
                CreateNotificationApplication(notification3, "Exporter two", UKCompetentAuthority.Wales, WasteType.CreateSrfWasteType(null)),
                CreateNotificationApplication(notification4, "Exporter RDF", UKCompetentAuthority.Wales, WasteType.CreateWoodWasteType(null, null)),
                CreateNotificationApplication(notification5, "not submitted", UKCompetentAuthority.England, WasteType.CreateWoodWasteType(null, null))
            });
        }

        private System.Data.Entity.DbSet<Domain.NotificationAssessment.NotificationAssessment> GetNotificationAssessments()
        {
            return helper.GetAsyncEnabledDbSet(new[]
            {
                CreateNotificationAssessment(notification1, NotificationStatus.Submitted),
                CreateNotificationAssessment(notification2, NotificationStatus.Submitted),
                CreateNotificationAssessment(notification3, NotificationStatus.Submitted),
                CreateNotificationAssessment(notification4, NotificationStatus.Submitted),
                CreateNotificationAssessment(notification5, NotificationStatus.NotSubmitted)
            });
        }

        private Domain.NotificationAssessment.NotificationAssessment CreateNotificationAssessment(Guid notificationApplicationId, NotificationStatus status)
        {
            var assessment = new Domain.NotificationAssessment.NotificationAssessment(notificationApplicationId);
            ObjectInstantiator<Domain.NotificationAssessment.NotificationAssessment>.SetProperty(n => n.Status, status, assessment);
            return assessment;
        }

        private NotificationApplication CreateNotificationApplication(Guid id, string exporterName, UKCompetentAuthority competentAuthority, WasteType wasteType)
        {
            var notificationApplication = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery, competentAuthority, 0);

            EntityHelper.SetEntityId(notificationApplication, id);

            var business = Business.CreateBusiness(exporterName, BusinessType.LimitedCompany, "irrelevant registration number", "irrelevant registration number");
            var address = ObjectFactory.CreateDefaultAddress();
            var contact = ObjectFactory.CreateEmptyContact();

            notificationApplication.SetExporter(business, address, contact);

            notificationApplication.SetWasteType(wasteType);

            return notificationApplication;
        }

        private async Task<IList<BasicSearchResult>> ResultsWhenSearchingFor(string searchTerm)
        {
            return await handler.HandleAsync(new GetBasicSearchResults(searchTerm));
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

            Assert.Equal(0, results.Count);
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

            Assert.Equal(4, results.Count);
        }

        [Fact]
        public async Task DoesNotReturnItemsWithStatusNotSubmitted()
        {
            var results = await ResultsWhenSearchingFor("GB 0");

            Assert.True(results.All(p => p.NotificationStatus != EnumHelper.GetDisplayName(NotificationStatus.NotSubmitted)));
        }
    }
}