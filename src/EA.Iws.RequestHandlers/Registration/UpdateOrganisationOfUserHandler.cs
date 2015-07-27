namespace EA.Iws.RequestHandlers.Registration
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.Registration;

    public class UpdateOrganisationOfUserHandler : IRequestHandler<UpdateOrganisationOfUser, bool>
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;

        public UpdateOrganisationOfUserHandler(IwsContext context, IUserContext userContext)
        {
            this.context = context;
            this.userContext = userContext;
        }

        public async Task<bool> HandleAsync(UpdateOrganisationOfUser command)
        {
            var user = await context.Users.SingleAsync(u => u.Id == userContext.UserId.ToString());
            var organisation = await context.Organisations.SingleAsync(o => o.Id == command.OrganisationId);

            user.UpdateOrganisationOfUser(organisation);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
