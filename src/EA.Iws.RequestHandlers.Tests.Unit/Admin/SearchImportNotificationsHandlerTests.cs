namespace EA.Iws.RequestHandlers.Tests.Unit.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using EA.Iws.DataAccess.Repositories.Imports;
    using EA.Iws.RequestHandlers.Mappings.ImportNotification;
    using EA.Prsd.Core.Helpers;
    using FakeItEasy;
    using Iws.Core.Admin.Search;
    using Iws.Core.ImportNotificationAssessment;
    using Iws.Core.Notification;
    using Iws.Core.Shared;
    using Iws.Domain;
    using Iws.Domain.ImportNotification;
    using Iws.RequestHandlers.Admin.Search;
    using Iws.Requests.Admin.Search;
    using Iws.TestHelpers.Helpers;
    using Prsd.Core.Domain;
    using Xunit;

    public class SearchImportNotificationsHandlerTests
    {
        private readonly Guid notification1 = new Guid("1A2A6255-A0B1-48B2-B248-D606B1BFB1DA");
        private readonly Guid notification2 = new Guid("879909D1-46BB-4396-8497-950EA4D29AB8");
        private readonly Guid notification3 = new Guid("268F7CBA-3146-4F88-9F07-A06B1BD36936");
        private readonly Guid notification4 = new Guid("D961B020-971E-44D4-8F27-C4B70791CAAA");
        private readonly Guid notification5 = new Guid("5C1DB181-2D02-42F5-A8BD-4EFA293007EC");
        private readonly Guid notification6 = new Guid("B0F061B0-E52A-4130-88DC-D23FF59BBF25");
        private readonly Guid notification7 = new Guid("ACECE188-5B7C-4A89-A406-416AE090C79B");
        private readonly SearchImportNotificationsHandler handler;
        private readonly IImportNotificationSearchRepository importNotificationSearchRepository;

        public SearchImportNotificationsHandlerTests()
        {
            var applications = GetImportNotifications();
            var assessments = GetNotificationAssessments();

            var impNotificationContext = new TestImportNotificationContext();
            var iwsContext = new TestIwsContext();
            var userContext = A.Fake<IUserContext>();
            var testMapper = new TestMapper();
            testMapper.AddMapper(new SearchResultMap());

            impNotificationContext.ImportNotifications.AddRange(applications);
            impNotificationContext.Importers.AddRange(GetImporters());
            impNotificationContext.ImportNotificationAssessments.AddRange(assessments);
            iwsContext.InternalUsers.AddRange(GetUsers());
            A.CallTo(() => userContext.UserId).Returns(new Guid("ac795e26-1563-4833-b8f9-0529eb9e66ae"));
            importNotificationSearchRepository = new ImportNotificationSearchRepository(iwsContext, impNotificationContext, userContext);

            handler = new SearchImportNotificationsHandler(importNotificationSearchRepository, testMapper);
        }

        private IEnumerable<ImportNotification> GetImportNotifications()
        {
            return new[]
            {
                CreateImportNotification(notification1, UKCompetentAuthority.England, "GB 0001 11111"),
                CreateImportNotification(notification2, UKCompetentAuthority.England, "GB 0001 22222"),
                CreateImportNotification(notification3, UKCompetentAuthority.England, "GB 0001 33333"),
                CreateImportNotification(notification4, UKCompetentAuthority.England, "GB 0001 44444"),
                CreateImportNotification(notification5, UKCompetentAuthority.England, "GB 1111 55555"),
                CreateImportNotification(notification6, UKCompetentAuthority.Scotland, "GB 0002 66666"),
                CreateImportNotification(notification7, UKCompetentAuthority.Wales, "GB 0004 77777")
            };
        }

        private IEnumerable<Importer> GetImporters()
        {
            var phoneNumber = new PhoneNumber("+44 7442226885");
            var emailAddress = new EmailAddress("Test@123.com");
            var address = new Address("Test Place", string.Empty, "Reading", "RG1 7BH", new Guid());
            var contact = new Contact("Test Contact", phoneNumber, emailAddress);

            return new[]
            {
                CreateImporter(notification1, "Importer 1", BusinessType.LimitedCompany, "11111", address, contact),
                CreateImporter(notification2, "Test Business 2", BusinessType.LimitedCompany, "22222", address, contact),
                CreateImporter(notification3, "Importer 2", BusinessType.LimitedCompany, "33333", address, contact),
                CreateImporter(notification4, "Test Business 4", BusinessType.LimitedCompany, "44444", address, contact),
                CreateImporter(notification5, "Importer 5", BusinessType.LimitedCompany, "55555", address, contact),
                CreateImporter(notification6, "Importer 6", BusinessType.SoleTrader, "66666", address, contact),
                CreateImporter(notification7, "Importer 7", BusinessType.Partnership, "77777", address, contact),
            };
        }

        private Importer CreateImporter(Guid importNotificationId, string businessName, BusinessType businessType, string registrationNumber, Address address, Contact contact)
        {
            var importer = new Importer(importNotificationId, businessName, businessType, registrationNumber, address, contact);
            return importer;
        }

        private IEnumerable<InternalUser> GetUsers()
        {
            var user = UserFactory.Create(new Guid("ac795e26-1563-4833-b8f9-0529eb9e66ae"), "Name", "Surname", "123456", "test@test.com");
            var internalUser = InternalUserFactory.Create(new Guid("9C67BFF3-6991-4188-9D8B-C989ADCE6E32"), user);
            ObjectInstantiator<InternalUser>.SetProperty(u => u.CompetentAuthority, UKCompetentAuthority.England, internalUser);

            var users = new[]
            {
                internalUser
            };
            return users;
        }

        private IEnumerable<Domain.ImportNotificationAssessment.ImportNotificationAssessment> GetNotificationAssessments()
        {
            return new[]
            {
                CreateNotificationAssessment(notification1, ImportNotificationStatus.Consented),
                CreateNotificationAssessment(notification2, ImportNotificationStatus.Consented),
                CreateNotificationAssessment(notification3, ImportNotificationStatus.Consented),
                CreateNotificationAssessment(notification4, ImportNotificationStatus.ConsentWithdrawn),
                CreateNotificationAssessment(notification5, ImportNotificationStatus.New),
                CreateNotificationAssessment(notification6, ImportNotificationStatus.Submitted),
                CreateNotificationAssessment(notification7, ImportNotificationStatus.Withdrawn)
            };
        }

        private Domain.ImportNotificationAssessment.ImportNotificationAssessment CreateNotificationAssessment(Guid notificationApplicationId, ImportNotificationStatus status)
        {
            var assessment = new Domain.ImportNotificationAssessment.ImportNotificationAssessment(notificationApplicationId);
            ObjectInstantiator<Domain.ImportNotificationAssessment.ImportNotificationAssessment>.SetProperty(n => n.Status, status, assessment);
            return assessment;
        }

        private ImportNotification CreateImportNotification(Guid id, UKCompetentAuthority competentAuthority, string notificationNumber)
        {
            var importNotification = new ImportNotification(NotificationType.Recovery, competentAuthority, notificationNumber);
            EntityHelper.SetEntityId(importNotification, id);
            return importNotification;
        }

        private async Task<IList<ImportSearchResult>> ResultsWhenSearchingFor(string searchTerm)
        {
            return await handler.HandleAsync(new SearchImportNotifications(searchTerm));
        }

        [Fact]
        public async Task SearchBy_NotificationNumber_AllDataMatches()
        {
            string searchNotificationNumber = "GB 0001 11111";
            var results = await ResultsWhenSearchingFor(searchNotificationNumber);

            Assert.Equal(1, results.Count);
            Assert.True(results.Any(r => r.NotificationNumber.Contains(searchNotificationNumber)));
        }

        [Fact]
        public async Task SearchBy_NotificationNumber_NoDataMatch()
        {
            string nonExistantSearchTerm = "GB 0001 005001";
            var results = await ResultsWhenSearchingFor(nonExistantSearchTerm);

            Assert.Equal(0, results.Count);
            Assert.True(!results.Any(r => r.NotificationNumber.Contains(nonExistantSearchTerm)));
        }

        [Fact]
        public async Task SearchBy_NotificationNumber_NotificationNumberWithoutSpace()
        {
            var results = await ResultsWhenSearchingFor("GB000144444");
            Assert.Equal(1, results.Count);
        }

        [Fact]
        public async Task SearchBy_ImporterName_Multiples()
        {
            var results = await ResultsWhenSearchingFor("Importer");
            Assert.Equal(3, results.Count);
        }

        [Fact]
        public async Task Find_NotAvailable_ImporterName()
        {
            var results = await ResultsWhenSearchingFor("Importer Six");
            Assert.Equal(0, results.Count);
        }

        [Fact]
        public async Task Find_Available_ImporterName()
        {
            var results = await ResultsWhenSearchingFor("Importer 2");
            Assert.Equal(1, results.Count);
        }

        [Fact]
        public async Task SearchWithPartialNumber()
        {
            var results = await ResultsWhenSearchingFor("GB 0001");
            Assert.Equal(4, results.Count);
        }

        [Fact]
        public async Task DoesNotReturnItemsWithStatusNew()
        {
            var results = await ResultsWhenSearchingFor("GB 0");
            Assert.True(results.All(p => p.Status.ToString() != EnumHelper.GetDisplayName(ImportNotificationStatus.New)));
        }

        [Fact]
        public async Task SearchExcludesDifferentCompetentAuthority()
        {
            var results = await ResultsWhenSearchingFor("GB");
            Assert.Equal(5, results.Count);
        }

        [Fact]
        public async Task Find_Available_ImporterName_With_UpperCase()
        {
            var results = await ResultsWhenSearchingFor("IMPORTER 2");
            Assert.Equal(1, results.Count);
        }

        [Fact]
        public async Task Find_Available_NotificationNumber_With_LowerCase()
        {
            var results = await ResultsWhenSearchingFor("gb 0001 11111");
            Assert.Equal(1, results.Count);
        }
    }
}
