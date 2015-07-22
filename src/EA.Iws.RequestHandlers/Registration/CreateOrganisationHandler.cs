namespace EA.Iws.RequestHandlers.Registration
{
    using System;
    using System.Data.Entity;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using DataAccess;
    using DataAccess.Identity;
    using Domain;
    using Microsoft.AspNet.Identity;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.Registration;
    using ClaimTypes = Core.Shared.ClaimTypes;

    internal class CreateOrganisationHandler : IRequestHandler<CreateOrganisation, Guid>
    {
        private readonly IwsContext db;
        private readonly IUserContext userContext;
        private readonly UserManager<ApplicationUser> userManager;

        public CreateOrganisationHandler(IwsContext db, IUserContext userContext, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.userContext = userContext;
            this.userManager = userManager;
        }

        public async Task<Guid> HandleAsync(CreateOrganisation command)
        {
            var orgData = command.Organisation;
            var country = await db.Countries.SingleAsync(c => c.Id == command.Organisation.CountryId);

            var address = new Address(orgData.Building, orgData.Address1,
                orgData.Address2, orgData.TownOrCity, null, orgData.Postcode, country.Name);
            var organisation = new Organisation(command.Organisation.Name, address, BusinessType.FromBusinessType(command.Organisation.BusinessType), command.Organisation.OtherDescription);

            db.Organisations.Add(organisation);

            await db.SaveChangesAsync();
            
            return organisation.Id;
        }
    }
}