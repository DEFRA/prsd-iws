namespace EA.Iws.RequestHandlers.Registration
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Prsd.Core.Mediator;
    using Requests.Registration;

    internal class CreateOrganisationHandler : IRequestHandler<CreateOrganisation, Guid>
    {
        private readonly IwsContext db;

        public CreateOrganisationHandler(IwsContext db)
        {
            this.db = db;
        }

        public async Task<Guid> HandleAsync(CreateOrganisation command)
        {
            var orgData = command.Organisation;
            var country = await db.Countries.SingleAsync(c => c.Id == new Guid(command.Organisation.Country));

            var address = new Address(orgData.Building, orgData.Address1, orgData.TownOrCity, orgData.Postcode, country,
                orgData.Address2);
            var organisation = new Organisation(command.Organisation.Name, address, command.Organisation.EntityType,
                command.Organisation.CompaniesHouseNumber);

            db.Organisations.Add(organisation);

            await db.SaveChangesAsync();

            return organisation.Id;
        }
    }
}