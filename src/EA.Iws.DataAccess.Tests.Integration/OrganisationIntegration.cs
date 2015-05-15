namespace EA.Iws.DataAccess.Tests.Integration
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using Domain;
    using FakeItEasy;
    using Prsd.Core.Domain;
    using Xunit;

    public class OrganisationIntegration
    {
        private readonly IwsContext context;

        public OrganisationIntegration()
        {
            var userContext = A.Fake<IUserContext>();

            A.CallTo(() => userContext.UserId).Returns(Guid.NewGuid());

            context = new IwsContext(userContext);
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

            var org = new Organisation("SFW Ltd", address, "Company");

            context.Organisations.Add(org);

            try
            {
                context.SaveChanges();
                CleanUp(org, address);
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

        private static Address TestAddress(Country country)
        {
            return new Address("1", "test street", null, "Woking", "GU22 7UM", country.Name);
        }

        private void CleanUp(Organisation organisation, Address address)
        {
            context.Organisations.Remove(organisation);

            context.SaveChanges();
        }
    }
}