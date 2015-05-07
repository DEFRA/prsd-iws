namespace EA.Iws.Cqrs.Registration
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.Cqrs;
    using DataAccess;
    using Domain;

    internal class CreateOrganisationHandler : ICommandHandler<CreateOrganisation>
    {
        private readonly IwsContext db;

        public CreateOrganisationHandler(IwsContext db)
        {
            this.db = db;
        }

        public async Task HandleAsync(CreateOrganisation command)
        {
            var orgData = command.Organisation;
            var country = await db.Countries.SingleAsync(c => c.Id == new Guid(command.Organisation.Country));

            var address = new Address(orgData.Building, orgData.Address1, orgData.TownOrCity, orgData.Postcode, country,
                orgData.Address2);
            var organisation = new Organisation(command.Organisation.Name, address, command.Organisation.EntityType,
                command.Organisation.CompaniesHouseNumber);

            db.Organisations.Add(organisation);

            await db.SaveChangesAsync();
        }
    }
}