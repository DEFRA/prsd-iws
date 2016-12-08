namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class UpdateInterimStatusHandler : IRequestHandler<UpdateInterimStatus, bool>
    {
        private readonly NotificationInterim notificationInterim;
        private readonly IwsContext context;

        public UpdateInterimStatusHandler(NotificationInterim notificationInterim, IwsContext context)
        {
            this.notificationInterim = notificationInterim;
            this.context = context;
        }

        public async Task<bool> HandleAsync(UpdateInterimStatus message)
        {
            await notificationInterim.SetValue(message.NotificationId, message.IsInterim);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
