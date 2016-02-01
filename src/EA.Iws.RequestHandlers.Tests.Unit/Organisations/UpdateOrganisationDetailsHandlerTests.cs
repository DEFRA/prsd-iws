namespace EA.Iws.RequestHandlers.Tests.Unit.Organisations
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.Registration;
    using Core.Shared;
    using DataAccess;
    using Domain;
    using Domain.NotificationApplication;
    using FakeItEasy;
    using Prsd.Core.Domain;
    using RequestHandlers.Organisations;
    using Requests.Organisations;
    using TestHelpers.Helpers;
    using Xunit;

    public class UpdateOrganisationDetailsHandlerTests
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;
        private readonly UpdateOrganisationDetailsHandler handler;
        private readonly Guid organisationId = new Guid("6414C995-7E20-48B7-AE37-5A8537D98645");
        private readonly Guid userId = new Guid("814C1C27-0DBA-46A9-914F-CD97FA2F3146");
        private readonly Guid countryId = new Guid("05C21C57-2F39-4A15-A09A-5F38CF139C05");
        private readonly Address address;

        private const string name = "org name";
        private const string address1 = "address line one";
        private const string address2 = "address line two";
        private const string town = "town";
        private const string postcode = "GU227UY";
        private const string otherDescription = "other business type";

        public UpdateOrganisationDetailsHandlerTests()
        {
            userContext = A.Fake<IUserContext>();
            A.CallTo(() => userContext.UserId).Returns(userId);

            context = new TestIwsContext(userContext);

            var country = CountryFactory.Create(countryId);
            context.Countries.Add(country);

            address = new Address(address1, address2, town, null, postcode, country.Name);

            context.Organisations.Add(GetOrganisation());

            context.Users.Add(GetUser());

            handler = new UpdateOrganisationDetailsHandler(context, userContext);
        }

        private User GetUser()
        {
            User user = UserFactory.Create(userId, "firstName", "lastName", "9123456789", "test@test.com");
            Organisation org = new Organisation(name, BusinessType.Other, otherDescription);
            user.LinkToAddress(new UserAddress(new Address(address1, address2, town, null, postcode, "United Kingdom")));

            EntityHelper.SetEntityId(org, organisationId);

            user.LinkToOrganisation(org);
            return user;
        }

        private Organisation GetOrganisation()
        {
            User user = UserFactory.Create(userId, "firstName", "lastName", "9123456789", "test@test.com");
            Organisation org = new Organisation(name, BusinessType.Other, otherDescription);
            EntityHelper.SetEntityId(org, organisationId);

            user.LinkToOrganisation(org);
            return org;
        }

        private OrganisationRegistrationData GetOrganisationRegistrationData(string prefix)
        {
            return new OrganisationRegistrationData()
            {
                OrganisationId = organisationId,
                Name = prefix + name,
                BusinessType = Core.Shared.BusinessType.Other,
                OtherDescription = otherDescription
            };
        }

        [Fact]
        public async Task UpdateDetails_Address_Changes_Address()
        {
            var prefix = "updated";
            var request = new UpdateOrganisationDetails(new OrganisationRegistrationData()
            {
                OrganisationId = organisationId,
                Name = prefix + name,
                BusinessType = Core.Shared.BusinessType.SoleTrader,
                Address = new AddressData { Address2 = address2, StreetOrSuburb = address1, CountryId = countryId, PostalCode = postcode, TownOrCity = town }
            });
            var orgId = await handler.HandleAsync(request);

            var user = await context.Users.SingleAsync(x => x.Id == userId.ToString());
            Assert.Equal(address1, user.Address.Address.Address1);
        }
    }
}
