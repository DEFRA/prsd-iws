namespace EA.Iws.RequestHandlers.Registration
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using DataAccess.Identity;
    using Domain;
    using Microsoft.AspNet.Identity;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.Registration;

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
            var organisation = new Organisation(command.Organisation.Name, command.Organisation.BusinessType, command.Organisation.OtherDescription);

            db.Organisations.Add(organisation);

            await db.SaveChangesAsync();

            return organisation.Id;
        }
    }
}