namespace EA.Iws.DataAccess.Tests.Integration.Search
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Admin.Search;
    using Core.Notification;
    using FakeItEasy;
    using Prsd.Core.Domain;
    using Repositories.Search;
    using Xunit;

    [Trait("Category", "Integration")]
    public class AdvancedSearchRepositoryIntegration
    {
        private readonly IwsContext context;

        public AdvancedSearchRepositoryIntegration()
        {
            var userContext = A.Fake<IUserContext>();

            A.CallTo(() => userContext.UserId).Returns(Guid.NewGuid());

            context = new IwsContext(userContext, A.Fake<IEventDispatcher>());
        }

        [Fact]
        public async Task CanPerformAdvancedSearchExportNotifications()
        {
            var advancedSearchRepository = new AdvancedSearchRepository(context);

            var criteria = new AdvancedSearchCriteria();

            var results = await advancedSearchRepository.SearchExportNotificationsByCriteria(criteria, UKCompetentAuthority.England);

            Assert.NotNull(results);
        }
    }
}