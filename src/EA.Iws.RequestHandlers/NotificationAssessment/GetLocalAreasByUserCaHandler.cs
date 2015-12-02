namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Admin;
    using DataAccess;
    using Domain;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class GetLocalAreasByUserCaHandler : IRequestHandler<GetLocalAreasByUserCa, List<LocalAreaData>>
    {
        private readonly IwsContext iwsContext;
        private readonly IUserContext userContext;
        private readonly IInternalUserRepository internalUserRepository;

        public GetLocalAreasByUserCaHandler(IwsContext iwsContext, 
            IUserContext userContext,
            IInternalUserRepository internalUserRepository)
        {
            this.iwsContext = iwsContext;
            this.userContext = userContext;
            this.internalUserRepository = internalUserRepository;
        }

        public async Task<List<LocalAreaData>> HandleAsync(GetLocalAreasByUserCa message)
        {
            var user = await internalUserRepository.GetByUserId(userContext.UserId);

            return await iwsContext.LocalAreas.OrderBy(x => x.Name).Where(x => x.CompetentAuthorityId == user.CompetentAuthority.Value)
                .Select(p => new LocalAreaData() { Id = p.Id, Name = p.Name, CompetentAuthorityId = p.CompetentAuthorityId })
                .ToListAsync();
        }
    }
}
