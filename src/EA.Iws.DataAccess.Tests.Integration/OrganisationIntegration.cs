namespace EA.Iws.DataAccess.Tests.Integration
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using Core.Domain;
    using Domain;
    using FakeItEasy;
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

            int count = countries.Count;
        }

        [Fact]
        public void CanFindCountryByEUMemberStatus()
        {
            var country = context.Countries.First(c => c.IsEuropeanUnionMember);

            Assert.True(country.IsEuropeanUnionMember);
        }

        [Fact]
        public void CanCreateAddress()
        {
            var country = context.Countries.Single(c => c.IsoAlpha2Code.Equals("GB"));

            var address = TestAddress(country);

            context.Addresses.Add(address);

            context.SaveChanges();

            CleanUp(address);
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
            catch (Exception ex)
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
            return new Address("1 Test Street", "Woking", "GU22 7UM", country);
        }

        private void CleanUp(Organisation organisation, Address address)
        {
            context.Organisations.Remove(organisation);

            context.SaveChanges();

            CleanUp(address);
        }

        private void CleanUp(Address address)
        {
            context.Addresses.Remove(address);

            context.SaveChanges();
        }
    }
}
