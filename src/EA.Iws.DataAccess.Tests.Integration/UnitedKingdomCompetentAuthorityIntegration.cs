namespace EA.Iws.DataAccess.Tests.Integration
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Notification;
    using FakeItEasy;
    using Prsd.Core.Domain;
    using Xunit;

    [Trait("Category", "Integration")]
    public class UnitedKingdomCompetentAuthorityIntegration
    {
        private readonly IwsContext context;

        public UnitedKingdomCompetentAuthorityIntegration()
        {
            context = new IwsContext(A.Fake<IUserContext>());
        }

        [Fact]
        public async Task CanRetrieveCompetentAuthorities()
        {
            var result = await context.UnitedKingdomCompetentAuthorities.ToArrayAsync();

            Assert.Equal("England", result.Single(ca => ca.Id == (int)CompetentAuthority.England).UnitedKingdomCountry);
        }
    }
}
