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
    using Requests.Registration.Users;
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

            var org = new Organisation("SFW Ltd", address, BusinessType.LimitedCompany);

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

        [Fact]
        public void UpdateExistingOrganisation()
        {
            var country = context.Countries.Single(c => c.IsoAlpha2Code.Equals("gb"));
            var address = TestAddress(country);
            var org = new Organisation("SFW Ltd", address, BusinessType.LimitedCompany);

            context.Organisations.Add(org);
            context.SaveChanges();

            var orgId = org.Id;
            org.Update("Only Name Changed", address, BusinessType.LimitedCompany);
            context.SaveChanges();
            Assert.True(orgId == org.Id);

            CleanUp(org, address);
        }

        [Fact]
        public async Task UpdateOrganisation_BusinessType_Removes_Organisation()
        {
            //Create new user
            var newUser = new User(userId.ToString(), "testFirst", "testLast", "9999", "testfirst@testlast.com");
            context.Users.Add(newUser);
            await context.SaveChangesAsync();

            //Create new org
            var country = context.Countries.Single(c => c.IsoAlpha2Code.Equals("gb"));
            var address = TestAddress(country);
            var org = new Organisation("SFW Ltd", address, BusinessType.LimitedCompany);
            context.Organisations.Add(org);
            await context.SaveChangesAsync();

            //Assign org to user
            newUser.LinkToOrganisation(org);
            await context.SaveChangesAsync();

            //Hold OrgID of newly created entity
            var oldOrgId = org.Id;

            //Update org with change in Business Type
            org = new Organisation("Name Changed", address, BusinessType.SoleTrader);
            context.Organisations.Add(org);
            await context.SaveChangesAsync();

            //Update user with newly created org2
            var user = await context.Users.SingleAsync(u => u.Id == newUser.Id.ToString());
            user.UpdateOrganisationOfUser(org);
            await context.SaveChangesAsync();
            Assert.True(user.Organisation.Id == org.Id);

            //Both Orgs should have different OrgIds
            Assert.False(oldOrgId == org.Id);

            //Check if old org exists
            var oldExists = context.Organisations.Any(x => x.Id == oldOrgId);
            Assert.Equal(oldExists, false);
            
            //Check if new org exists
            var newExists = context.Organisations.Any(x => x.Id == org.Id);
            Assert.True(newExists);

            //CleanUp(org, address);
        }

        [Fact]
        public async Task UpdateOrganisation_BusinessType_Should_Not_Remove_Organisation()
        {
            //Create new user
            var newUser = new User(userId.ToString(), "testFirst", "testLast", "9999", "testfirst@testlast.com");
            context.Users.Add(newUser);
            await context.SaveChangesAsync();

            //Create new org
            var country = context.Countries.Single(c => c.IsoAlpha2Code.Equals("gb"));
            var address = TestAddress(country);
            var org = new Organisation("SFW Ltd", address, BusinessType.LimitedCompany);
            context.Organisations.Add(org);
            await context.SaveChangesAsync();

            //Assign org to user
            newUser.LinkToOrganisation(org);
            await context.SaveChangesAsync();

            //Hold OrgID of newly created entity
            var oldOrgId = org.Id;

            //Update org with change in Business Type
            org = new Organisation("Name Changed", address, BusinessType.SoleTrader);
            context.Organisations.Add(org);
            await context.SaveChangesAsync();

            //Update user with newly created org2
            var user = await context.Users.SingleAsync(u => u.Id == newUser.Id.ToString());
            user.UpdateOrganisationOfUser(org);
            await context.SaveChangesAsync();
            Assert.True(user.Organisation.Id == org.Id);

            //Both Orgs should have different OrgIds
            Assert.False(oldOrgId == org.Id);

            //Check if old org exists
            var oldExists = context.Organisations.Any(x => x.Id == oldOrgId);
            Assert.True(oldExists);

            //Check if new org exists
            var newExists = context.Organisations.Any(x => x.Id == org.Id);
            Assert.True(newExists);

            //CleanUp(org, address);
        }

        private static Address TestAddress(Country country)
        {
            return new Address("1", "test street", null, "Woking", null, "GU22 7UM", country.Name);
        }

        private void CleanUp(Organisation organisation, Address address)
        {
            context.Organisations.Remove(organisation);

            context.SaveChanges();
        }
    }
}