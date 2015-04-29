namespace EA.Iws.Cqrs.Registration
{
    using System.Threading.Tasks;
    using Core.Cqrs;
    using DataAccess;

    public class LinkUserToOrganisationHandler : ICommandHandler<LinkUserToOrganisation>
    {
        private readonly IwsContext context;

        public LinkUserToOrganisationHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task HandleAsync(LinkUserToOrganisation command)
        {
            var user = await context.Users.FindAsync(command.UserId);

            user.LinkToOrganisation(command.Organisation);

            await context.SaveChangesAsync();
        }
    }
}
