namespace EA.Iws.RequestHandlers.Admin
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Admin;
    using Domain;
    using Prsd.Core.Domain;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Admin;

    internal class GetExportNotificationOwnerDisplaysHandler : IRequestHandler<GetExportNotificationOwnerDisplays, IEnumerable<ExportNotificationOwnerDisplay>>
    {
        private readonly IMapper mapper;
        private readonly Domain.NotificationApplication.IExportNotificationOwnerDisplayRepository exportOwnerDisplayRepository;
        private readonly IInternalUserRepository internalUserRepository;
        private readonly IUserContext userContext;

        public GetExportNotificationOwnerDisplaysHandler(IUserContext userContext,
            IInternalUserRepository internalUserRepository,
            Domain.NotificationApplication.IExportNotificationOwnerDisplayRepository exportOwnerDisplayRepository,
            IMapper mapper)
        {
            this.userContext = userContext;
            this.internalUserRepository = internalUserRepository;
            this.exportOwnerDisplayRepository = exportOwnerDisplayRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ExportNotificationOwnerDisplay>> HandleAsync(GetExportNotificationOwnerDisplays message)
        {
            var currentUser = await internalUserRepository.GetByUserId(userContext.UserId);

            var result = (await exportOwnerDisplayRepository
                .GetInternalUnsubmittedByCompetentAuthority(currentUser.CompetentAuthority));

            return result.Select(x => mapper.Map<ExportNotificationOwnerDisplay>(x)).ToArray();
        }
    }
}