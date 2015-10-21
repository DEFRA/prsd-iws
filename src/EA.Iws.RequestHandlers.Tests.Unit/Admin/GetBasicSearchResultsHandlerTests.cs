namespace EA.Iws.RequestHandlers.Tests.Unit.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Admin.Search;
    using Core.NotificationAssessment;
    using Core.Shared;
    using Domain;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Exporter;
    using FakeItEasy;
    using Prsd.Core.Domain;
    using Prsd.Core.Helpers;
    using RequestHandlers.Admin.Search;
    using Requests.Admin;
    using TestHelpers.DomainFakes;
    using TestHelpers.Helpers;
    using Xunit;

    public class GetBasicSearchResultsHandlerTests
    {
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

            var context = new TestIwsContext();
            var userContext = A.Fake<IUserContext>();

            context.NotificationApplications.AddRange(applications);
            context.Exporters.AddRange(GetExporters());
            context.NotificationAssessments.AddRange(assessments);
            context.InternalUsers.AddRange(GetUsers());
            A.CallTo(() => userContext.UserId).Returns(new Guid("ac795e26-1563-4833-b8f9-0529eb9e66ae"));

            handler = new GetBasicSearchResultsHandler(context, userContext);
        }

        private IEnumerable<NotificationApplication> GetNotificationApplications()
        {
            return new[]
            {
                CreateNotificationApplication(notification1, UKCompetentAuthority.England, WasteType.CreateRdfWasteType(null)),
                CreateNotificationApplication(notification2, UKCompetentAuthority.England, WasteType.CreateRdfWasteType(null)),
                CreateNotificationApplication(notification3, UKCompetentAuthority.England, WasteType.CreateSrfWasteType(null)),
                CreateNotificationApplication(notification4, UKCompetentAuthority.England, WasteType.CreateWoodWasteType(null, null)),
                CreateNotificationApplication(notification5, UKCompetentAuthority.England, WasteType.CreateWoodWasteType(null, null)),
                CreateNotificationApplication(notification5, UKCompetentAuthority.England, WasteType.CreateWoodWasteType(null, null))
            };
        }

        private IEnumerable<Exporter> GetExporters()
        {
            return new[]
            {
                CreateExporter(notification1, "Exporter one"),
                CreateExporter(notification2, "GB 0001 000000"),
                CreateExporter(notification3, "Exporter two"),
                CreateExporter(notification4, "Exporter RDF"),
                CreateExporter(notification5, "not submitted"),
                CreateExporter(notification5, "Exporter")
            };
        }

        private Exporter CreateExporter(Guid notificationId, string exporterName)
        {
            var business = (TestableBusiness)TestableBusiness.LargeObjectHeap;
            business.Name = exporterName;

            var exporter = new TestableExporter
            {
                NotificationId = notificationId,
                Address = TestableAddress.SouthernHouse,
                Contact = TestableContact.BillyKnuckles,
                Business = business
            };

            return exporter;
        }

        private IEnumerable<InternalUser> GetUsers()
        {
            var user = UserFactory.Create(new Guid("ac795e26-1563-4833-b8f9-0529eb9e66ae"), "Name", "Surname", "123456", "test@test.com");

            var internalUser = InternalUserFactory.Create(new Guid("9C67BFF3-6991-4188-9D8B-C989ADCE6E32"),  user);
            ObjectInstantiator<InternalUser>.SetProperty(u => u.CompetentAuthority, UKCompetentAuthority.England, internalUser);

            var users = new[]
            {
                internalUser
            };
            return users;
        }

        private IEnumerable<Domain.NotificationAssessment.NotificationAssessment> GetNotificationAssessments()
        {
            return new[]
            {
                CreateNotificationAssessment(notification1, NotificationStatus.Submitted),
                CreateNotificationAssessment(notification2, NotificationStatus.Submitted),
                CreateNotificationAssessment(notification3, NotificationStatus.Submitted),
                CreateNotificationAssessment(notification4, NotificationStatus.Submitted),
                CreateNotificationAssessment(notification5, NotificationStatus.NotSubmitted)
            };
        }

        private Domain.NotificationAssessment.NotificationAssessment CreateNotificationAssessment(Guid notificationApplicationId, NotificationStatus status)
        {
            var assessment = new Domain.NotificationAssessment.NotificationAssessment(notificationApplicationId);
            ObjectInstantiator<Domain.NotificationAssessment.NotificationAssessment>.SetProperty(n => n.Status, status, assessment);
            return assessment;
        }

        private NotificationApplication CreateNotificationApplication(Guid id, UKCompetentAuthority competentAuthority, WasteType wasteType)
        {
            var notificationApplication = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery, competentAuthority, 0);

            EntityHelper.SetEntityId(notificationApplication, id);
            
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

            Assert.Equal(4, results.Count);
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

            Assert.Equal(4, results.Count);
        }

        [Fact]
        public async Task SearchBy_ExporterName()
        {
            var results = await ResultsWhenSearchingFor("Exporter");

            Assert.Equal(3, results.Count);
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

        [Fact]
        public async Task SearchExcludesDifferenctCompetentAuthority()
        {
            var results = await ResultsWhenSearchingFor("GB 0001");
            Assert.Equal(4, results.Count);
        }
    }
}