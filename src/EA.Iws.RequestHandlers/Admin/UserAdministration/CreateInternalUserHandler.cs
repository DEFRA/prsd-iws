namespace EA.Iws.RequestHandlers.Admin.UserAdministration
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Prsd.Core.Mediator;
    using Requests.Admin.UserAdministration;

    internal class CreateInternalUserHandler : IRequestHandler<CreateInternalUser, Guid>
    {
        private readonly IwsContext context;

        public CreateInternalUserHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(CreateInternalUser message)
        {
            var internalUser = new InternalUser(message.UserId, message.JobTitle,
                UKCompetentAuthority.FromCompetentAuthority(message.CompetentAuthority), message.LocalAreaId);

            context.InternalUsers.Add(internalUser);

            await context.SaveChangesAsync();

            return internalUser.Id;
        }
    }
}