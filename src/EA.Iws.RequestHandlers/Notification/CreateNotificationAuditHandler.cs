namespace EA.Iws.RequestHandlers.Notification
{
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using System.Threading.Tasks;

    public class CreateNotificationAuditHandler : IRequestHandler<CreateNotificationAudit, bool>
    {
        private readonly IwsContext context;
        private readonly INotificationAuditRepository repository;

        public CreateNotificationAuditHandler(IwsContext context, INotificationAuditRepository repository)
        {
            this.context = context;
            this.repository = repository;
        }

        public async Task<bool> HandleAsync(CreateNotificationAudit message)
        {
            await this.repository.AddNotificationAudit(this.Map(message));

            return true;
        }

        private Audit Map(CreateNotificationAudit audit)
        {
            return new Audit(audit.NotificationId, audit.UserId, audit.Screen, (int)audit.Type, audit.DateAdded);
        }
    }
}
