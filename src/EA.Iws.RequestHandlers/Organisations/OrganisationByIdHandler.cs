namespace EA.Iws.RequestHandlers.Organisations
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Organisations;
    using Core.Shared;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Organisations;
    using Requests.Shared;

    internal class OrganisationByIdHandler : IRequestHandler<OrganisationById, OrganisationData>
    {
        private readonly IwsContext context;

        public OrganisationByIdHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<OrganisationData> HandleAsync(OrganisationById query)
        {
            return await context
                .Organisations
                .Select(o => new OrganisationData
                {
                    Address = new AddressData
                    {
                        Address2 = o.Address.Address2,
                        CountryName = o.Address.Country,
                        PostalCode = o.Address.PostalCode,
                        StreetOrSuburb = o.Address.Address1,
                        TownOrCity = o.Address.TownOrCity
                    },
                    Id = o.Id,
                    Name = o.Name
                }).SingleAsync();
        }
    }
}