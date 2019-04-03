namespace EA.Iws.RequestHandlers.Notification
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Notification.Audit;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GetNotificationAuditHandler :
        IRequestHandler<GetNotificationAudits, IEnumerable<NotificationAuditForDisplay>>
    {
        private readonly IwsContext context;
        private readonly IMapper mapper;
        private readonly INotificationAuditRepository repository;

        public GetNotificationAuditHandler(IwsContext context,
            IMapper mapper, INotificationAuditRepository repository)
        {
            this.context = context;
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<IEnumerable<NotificationAuditForDisplay>> HandleAsync(GetNotificationAudits message)
        {
            IEnumerable<Audit> notificationAudits = await repository.GetNotificationAuditsById(message.NotificationId);

            return notificationAudits
                .Select(updateHistoryItem => mapper.Map<NotificationAuditForDisplay>(updateHistoryItem))
                .OrderByDescending(x => x.DateAdded)
                .ToList();
        }
    }
}
