namespace EA.Iws.Cqrs.Registration
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain;
    using Iws.Core.Cqrs;
    using Iws.DataAccess;

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
            
            var address = new Address(orgData.Building, orgData.Address1, orgData.TownOrCity, orgData.Postcode, country, orgData.Address2);
            var organisation = new Organisation(command.Organisation.Name, address, command.Organisation.EntityType, command.Organisation.CompaniesHouseNumber);

            using (var transaction = db.Database.BeginTransaction())
            {
                db.Organisations.Add(organisation);

                try
                {
                    await db.SaveChangesAsync();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
        }
    }
}