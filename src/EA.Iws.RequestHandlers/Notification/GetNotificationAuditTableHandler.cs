namespace EA.Iws.RequestHandlers.Notification
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Notification.Audit;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GetNotificationAuditTableHandler : 
        IRequestHandler<GetNotificationAuditTable, NotificationAuditTable>
    {
        private readonly IwsContext context;
        private readonly IMapper mapper;
        private readonly INotificationAuditRepository repository;

        private const int PageSize = 10;

        public GetNotificationAuditTableHandler(IwsContext context, IMapper mapper, 
            INotificationAuditRepository repository)
        {
            this.context = context;
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<NotificationAuditTable> HandleAsync(GetNotificationAuditTable message)
        {
            IEnumerable<Audit> notificationAudits = await repository.GetPagedNotificationAuditsById(message.NotificationId, message.PageNumber, PageSize, message.Screen, message.StartDate, message.EndDate);

            var notificationAuditTable = mapper.Map<IEnumerable<Audit>, NotificationAuditTable>(notificationAudits);
            notificationAuditTable.PageNumber = message.PageNumber;
            notificationAuditTable.PageSize = PageSize;
            notificationAuditTable.NumberOfNotificationAudits = await repository.GetTotalNumberOfNotificationAudits(message.NotificationId);
            notificationAuditTable.NumberOfFilteredNotificationAudits = await repository.GetTotalNumberOfFilteredAudits(message.NotificationId, message.Screen, message.StartDate, message.EndDate);

            return notificationAuditTable;
        }
    }
}
