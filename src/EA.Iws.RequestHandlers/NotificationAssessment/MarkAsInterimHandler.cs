namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class MarkAsInterimHandler : IRequestHandler<MarkAsInterim, bool>
    {
        private readonly NotificationInterim notificationInterim;
        private readonly IwsContext context;

        public MarkAsInterimHandler(NotificationInterim notificationInterim, IwsContext context)
        {
            this.notificationInterim = notificationInterim;
            this.context = context;
        }

        public async Task<bool> HandleAsync(MarkAsInterim message)
        {
            await notificationInterim.SetValue(message.NotificationId, message.IsInterim);

            await context.SaveChangesAsync();

            return true;
        }
    }
}