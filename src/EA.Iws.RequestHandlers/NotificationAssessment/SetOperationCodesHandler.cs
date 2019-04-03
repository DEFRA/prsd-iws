namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Threading.Tasks;
    using DataAccess;
    using EA.Iws.Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class SetOperationCodesHandler : IRequestHandler<SetOperationCodes, bool>
    {
        private readonly IwsContext context;
        private readonly INotificationApplicationRepository notificationRepository;

        public SetOperationCodesHandler(IwsContext context, INotificationApplicationRepository notificationRepository)
        {
            this.context = context;
            this.notificationRepository = notificationRepository;
        }

        public async Task<bool> HandleAsync(SetOperationCodes message)
        {
            var notification = await notificationRepository.GetById(message.NotificationId);

            notification.SetOperationCodes(message.OperationCodes);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
