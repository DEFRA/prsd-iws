namespace EA.Iws.RequestHandlers.Organisations
{
    using System;
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
            var country = await context.Countries.SingleAsync(c => c.Name.Equals(org.Address.Country, StringComparison.InvariantCultureIgnoreCase));

            return new OrganisationRegistrationData()
            {
                OrganisationId = org.Id,
                Name = org.Name,
                Address1 = org.Address.Address1,
                Address2 = org.Address.Address2,
                TownOrCity = org.Address.TownOrCity,
                Postcode = org.Address.PostalCode,
                CountryId = country.Id,
                BusinessType = GetBusinessType(org.Type),
                OtherDescription = org.OtherDescription,
                RegistrationNumber = org.RegistrationNumber
            };
        }

        private static BusinessType GetBusinessType(string businessType)
        {
            if (businessType.Equals(Domain.BusinessType.SoleTrader.DisplayName, StringComparison.OrdinalIgnoreCase))
            {
                return BusinessType.SoleTrader;
            }
            if (businessType.Equals(Domain.BusinessType.Partnership.DisplayName, StringComparison.OrdinalIgnoreCase))
            {
                return BusinessType.Partnership;
            }
            if (businessType.Equals(Domain.BusinessType.LimitedCompany.DisplayName, StringComparison.OrdinalIgnoreCase))
            {
                return BusinessType.LimitedCompany;
            }
            if (businessType.Equals(Domain.BusinessType.Other.DisplayName, StringComparison.OrdinalIgnoreCase))
            {
                return BusinessType.Other;
            }

            throw new ArgumentException(string.Format("Unknown business type: {0}", businessType), "businessType");
        }
    }
}
