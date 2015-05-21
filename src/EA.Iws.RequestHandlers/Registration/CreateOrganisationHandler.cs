namespace EA.Iws.RequestHandlers.Registration
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using DataAccess.Identity;
    using Domain;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests;
    using Requests.Registration;

    internal class CreateOrganisationHandler : IRequestHandler<CreateOrganisation, Guid>
    {
        private readonly IwsContext db;
        private readonly IUserContext userContext;
        private readonly IwsIdentityContext identityContext;

        public CreateOrganisationHandler(IwsContext db, IUserContext userContext, IwsIdentityContext identityContext)
        {
            this.db = db;
            this.userContext = userContext;
            this.identityContext = identityContext;
        }

        public async Task<Guid> HandleAsync(CreateOrganisation command)
        {
            var orgData = command.Organisation;
            var country = await db.Countries.SingleAsync(c => c.Id == command.Organisation.CountryId);

            var address = new Address(orgData.Building, orgData.Address1,
                orgData.Address2, orgData.TownOrCity, orgData.Postcode, country.Name);
            var organisation = new Organisation(command.Organisation.Name, address, command.Organisation.EntityType,
                command.Organisation.CompaniesHouseNumber);

            db.Organisations.Add(organisation);

            await db.SaveChangesAsync();

            var user = await identityContext.Users.SingleAsync(u => u.Id.Equals(userContext.UserId.ToString()));
            user.Claims.Add(new IdentityUserClaim() { ClaimType = ClaimTypes.OrganisationId, ClaimValue = organisation.Id.ToString() });

            await identityContext.SaveChangesAsync();

            return organisation.Id;
        }
    }
}