namespace EA.Iws.RequestHandlers.Registration
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using DataAccess;
    using DataAccess.Identity;
    using Microsoft.AspNet.Identity;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.Registration;
    using ClaimTypes = Requests.ClaimTypes;

    internal class LinkUserToOrganisationHandler : IRequestHandler<LinkUserToOrganisation, bool>
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;
        private readonly UserManager<ApplicationUser> userManager;

        public LinkUserToOrganisationHandler(IwsContext context, IUserContext userContext, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userContext = userContext;
            this.userManager = userManager;
        }

        public async Task<bool> HandleAsync(LinkUserToOrganisation command)
        {
            var user = await context.Users.FindAsync(userContext.UserId.ToString());
            var organisation = await context.Organisations.FindAsync(command.OrganisationId);

            user.LinkToOrganisation(organisation);

            await context.SaveChangesAsync();

            await userManager.AddClaimAsync(userContext.UserId.ToString(), new Claim(ClaimTypes.OrganisationId, organisation.Id.ToString()));

            return true;
        }
    }
}