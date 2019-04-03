namespace EA.Iws.RequestHandlers.Notification
{
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using System.Threading.Tasks;

    public class CreateNotificationAuditHandler : IRequestHandler<CreateNotificationAudit, bool>
    {
        private readonly INotificationAuditRepository repository;

        public CreateNotificationAuditHandler(INotificationAuditRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> HandleAsync(CreateNotificationAudit message)
        {
            await repository.AddNotificationAudit(this.Map(message));

            return true;
        }

        private Audit Map(CreateNotificationAudit audit)
        {
            return new Audit(audit.NotificationId, audit.UserId, (int)audit.Screen, (int)audit.Type, audit.DateAdded);
        }
    }
}
