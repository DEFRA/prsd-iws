namespace EA.Iws.RequestHandlers.Admin.NotificationAssessment
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Admin.NotificationAssessment;

    internal class SetNotificationTransmittedDateHandler : IRequestHandler<SetNotificationTransmittedDate, bool>
    {
        private readonly IwsContext context;

        public SetNotificationTransmittedDateHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<bool> HandleAsync(SetNotificationTransmittedDate message)
        {
            var assessment =
                await
                    context.NotificationAssessments.SingleAsync(
                        p => p.NotificationApplicationId == message.NotificationId);

            assessment.Transmit(message.NotificationTransmittedDate);

            await context.SaveChangesAsync();

            return true;
        }
    }
}