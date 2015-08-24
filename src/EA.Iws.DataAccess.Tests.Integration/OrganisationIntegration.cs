namespace EA.Iws.DataAccess.Tests.Integration
{
    using System;
    using System.Data.Entity;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain;
    using FakeItEasy;
    using Prsd.Core.Domain;
    using Xunit;

    [Trait("Category", "Integration")]
    public class OrganisationIntegration
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;
        private readonly Guid userId = Guid.NewGuid();

        public OrganisationIntegration()
        {
            userContext = A.Fake<IUserContext>();
            A.CallTo(() => userContext.UserId).Returns(userId);
            context = new IwsContext(userContext, A.Fake<IEventDispatcher>());
        }

        [Fact]
        public void GetCountries()
        {
            var countries = context.Countries.ToList();

            var count = countries.Count;
        }

        [Fact]
        public void IsUkAddress()
        {
            var countryNonUk = context.Countries.First(c => !c.IsoAlpha2Code.Equals("gb"));

            var countryUk = context.Countries.Single(c => c.IsoAlpha2Code.Equals("gb"));

            var addressNonUk = TestAddress(countryNonUk);

            var addressUk = TestAddress(countryUk);

            Assert.True(addressUk.IsUkAddress);
            Assert.False(addressNonUk.IsUkAddress);
        }

        [Fact]
        public void CanCreateOrganisation()
        {
            var country = context.Countries.Single(c => c.IsoAlpha2Code.Equals("gb"));

            var address = TestAddress(country);

            var org = new Organisation("SFW Ltd", BusinessType.LimitedCompany);

            context.Organisations.Add(org);

            try
            {
                context.SaveChanges();
                CleanUp(org);
            }
            catch (Exception)
            {
                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }
                throw;
            }
        }

        [Fact]
        public void UpdateExistingOrganisation()
        {
            var country = context.Countries.Single(c => c.IsoAlpha2Code.Equals("gb"));
            var address = TestAddress(country);
            var org = new Organisation("SFW Ltd", BusinessType.LimitedCompany);

            context.Organisations.Add(org);
            context.SaveChanges();

            var orgId = org.Id;
            org.Update("Only Name Changed", address, BusinessType.LimitedCompany);
            context.SaveChanges();
            Assert.Equal(orgId, org.Id);

            CleanUp(org);
        }

        private static Address TestAddress(Country country)
        {
            return new Address("test street", null, "Woking", null, "GU22 7UM", country.Name);
        }

        private void CleanUp(Organisation organisation)
        {
            context.Organisations.Remove(organisation);

            context.SaveChanges();
        }
    }
}