namespace EA.Iws.Cqrs.Registration
{
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;

    public class LinkUserToOrganisationHandler : IRequestHandler<LinkUserToOrganisation, bool>
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;

        public LinkUserToOrganisationHandler(IwsContext context, IUserContext userContext)
        {
            this.context = context;
            this.userContext = userContext;
        }

        public async Task<bool> HandleAsync(LinkUserToOrganisation command)
        {
            var user = await context.Users.FindAsync(userContext.UserId);
            var organisation = await context.Organisations.FindAsync(command.OrganisationId);

            user.LinkToOrganisation(organisation);

            await context.SaveChangesAsync();

            return true;
        }
    }
}