namespace EA.Iws.RequestHandlers.Tests.Unit.Organisations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.Registration;
    using DataAccess;
    using Domain;
    using FakeItEasy;
    using Helpers;
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
            context = A.Fake<IwsContext>();
            userContext = A.Fake<IUserContext>();

            var dbContextHelper = new DbContextHelper();

            var country = CountryFactory.Create(countryId);
            A.CallTo(() => context.Countries).Returns(dbContextHelper.GetAsyncEnabledDbSet(new[] { country }));

            address = new Address(address1, address2, town, null, postcode, country.Name);

            A.CallTo(() => context.Organisations).Returns(dbContextHelper.GetAsyncEnabledDbSet(new List<Organisation> { GetOrganisation() }));

            A.CallTo(() => context.Users).Returns(dbContextHelper.GetAsyncEnabledDbSet(new[] { GetUser() }));
            A.CallTo(() => userContext.UserId).Returns(userId);

            new UpdateOrganisationDetails(GetOrganisationRegistrationData("my"));
            handler = new UpdateOrganisationDetailsHandler(context, userContext);
        }

        private User GetUser()
        {
            User user = new User(userId.ToString(), "firstName", "lastName", "9123456789", "test@test.com");
            Organisation org = new Organisation(name, address, BusinessType.Other, otherDescription);
            EntityHelper.SetEntityId(org, organisationId);

            user.LinkToOrganisation(org);
            return user;
        }

        private Organisation GetOrganisation()
        {
            User user = new User(userId.ToString(), "firstName", "lastName", "9123456789", "test@test.com");
            Organisation org = new Organisation(name, address, BusinessType.Other, otherDescription);
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
                Address1 = prefix + address1,
                Address2 = prefix + address2,
                TownOrCity = prefix + town,
                Postcode = postcode,
                CountryId = countryId,
                BusinessType = Core.Shared.BusinessType.Other,
                OtherDescription = otherDescription
            };
        }

        [Fact]
        public async Task UpdateOrgDetails_BusinessType_Changes_OrgId()
        {
            var prefix = "updated";
            var request = new UpdateOrganisationDetails(new OrganisationRegistrationData()
            {
                OrganisationId = organisationId,
                Name = prefix + name,
                Address1 = prefix + address1,
                Address2 = prefix + address2,
                TownOrCity = prefix + town,
                Postcode = "AB142TR",
                CountryId = countryId,
                BusinessType = Core.Shared.BusinessType.SoleTrader
            });
            var orgId = await handler.HandleAsync(request);

            var user = await context.Users.SingleAsync(x => x.Id == userId.ToString());
            Assert.Equal(orgId, user.Organisation.Id);
        }
    }
}
