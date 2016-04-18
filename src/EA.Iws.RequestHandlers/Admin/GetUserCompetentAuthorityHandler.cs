namespace EA.Iws.RequestHandlers.Admin
{
    using System.Threading.Tasks;
    using Core.Notification;
    using Domain;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.Admin;

    internal class GetUserCompetentAuthorityHandler : IRequestHandler<GetUserCompetentAuthority, UKCompetentAuthority>
    {
        private readonly IInternalUserRepository internalUserRepository;
        private readonly IUserContext userContext;

        public GetUserCompetentAuthorityHandler(IUserContext userContext,
            IInternalUserRepository internalUserRepository)
        {
            this.userContext = userContext;
            this.internalUserRepository = internalUserRepository;
        }

        public async Task<UKCompetentAuthority> HandleAsync(GetUserCompetentAuthority message)
        {
            return (await internalUserRepository.GetByUserId(userContext.UserId)).CompetentAuthority;
        }
    }
}