namespace EA.Iws.RequestHandlers.Organisations
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using System.Linq;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Organisations;

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
                        Building = o.Address.Building,
                        Country = o.Address.Country.Name,
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