namespace EA.Iws.RequestHandlers.Tests.Unit.Organisations
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using FakeItEasy;
    using Helpers;
    using Prsd.Core.Domain;
    using RequestHandlers.Organisations;
    using Requests.Organisations;
    using TestHelpers.Helpers;
    using Xunit;

    public class GetOrganisationDetailsByUserHandlerTests
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;
        private readonly GetOrganisationDetailsByUser message;
        private readonly GetOrganisationDetailsByUserHandler handler;
        private readonly Guid organisationId = new Guid("C0A5E713-61F0-46DA-AEB6-D0602E5B3ED6");
        private readonly Guid userId = new Guid("714E6E4F-C5D2-4BD8-8C1A-1032452C55EC");
        private Address address;

        private string name = "My org name";
        private string address1 = "address line one";
        private string address2 = "address line two";
        private string town = "Mytown";
        private string postcode = "GU227UY";
        private string otherDescription = "other business type";

        public GetOrganisationDetailsByUserHandlerTests()
        {
            context = A.Fake<IwsContext>();
            userContext = A.Fake<IUserContext>();

            A.CallTo(() => userContext.UserId).Returns(userId);

            var dbContextHelper = new DbContextHelper();

            var country = CountryFactory.Create(new Guid("05C21C57-2F39-4A15-A09A-5F38CF139C05"));
            A.CallTo(() => context.Countries).Returns(dbContextHelper.GetAsyncEnabledDbSet(new[] { country }));

            address = new Address(address1, address2, town, null, postcode, country.Name);

            A.CallTo(() => context.Organisations).Returns(dbContextHelper.GetAsyncEnabledDbSet(new[] { GetOrganisation() }));

            A.CallTo(() => context.Users).Returns(dbContextHelper.GetAsyncEnabledDbSet(new[] { GetUser() }));

            message = new GetOrganisationDetailsByUser();
            handler = new GetOrganisationDetailsByUserHandler(context, userContext);
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

        [Fact]
        public async Task GetOrgDetails_Verify_Original_Values()
        {
            var result = await handler.HandleAsync(message);
            Assert.True(result.Name == name
                        && result.Address1 == address1 && result.Address2 == address2
                        && result.TownOrCity == town && result.Postcode == postcode
                        && result.BusinessType == Core.Shared.BusinessType.Other && result.OtherDescription == otherDescription);
        }
    }
}
