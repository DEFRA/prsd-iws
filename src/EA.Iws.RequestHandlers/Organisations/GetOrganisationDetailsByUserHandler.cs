namespace EA.Iws.RequestHandlers.Organisations
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.Registration;
    using Core.Shared;
    using DataAccess;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.Organisations;

    internal class GetOrganisationDetailsByUserHandler : IRequestHandler<GetOrganisationDetailsByUser, OrganisationRegistrationData>
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;

        public GetOrganisationDetailsByUserHandler(IwsContext context, IUserContext userContext)
        {
            this.context = context;
            this.userContext = userContext;
        }

        public async Task<OrganisationRegistrationData> HandleAsync(GetOrganisationDetailsByUser query)
        {
            var userId = userContext.UserId.ToString();
            var user = await context.Users.SingleAsync(x => x.Id == userId);
            var org = user.Organisation;
            var userAddress = user.Address;

            return new OrganisationRegistrationData()
            {
                OrganisationId = org.Id,
                Name = org.Name,
                BusinessType = (BusinessType)org.Type,
                OtherDescription = org.OtherDescription,
                RegistrationNumber = org.RegistrationNumber,
                Address = new AddressData
                {
                    CountryName = userAddress.Address.Country,
                    Address2 = userAddress.Address.Address2,
                    PostalCode = userAddress.Address.PostalCode,
                    Region = userAddress.Address.Region,
                    StreetOrSuburb = userAddress.Address.Address1,
                    TownOrCity = userAddress.Address.TownOrCity
                }
            };
        }
    }
}
