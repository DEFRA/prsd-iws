namespace EA.Iws.RequestHandlers.Admin.NotificationAssessment
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Admin.NotificationAssessment;

    internal class SetNotificationDecisionRequiredByDateHandler : IRequestHandler<SetNotificationDecisionRequiredByDate, bool>
    {
        private readonly IwsContext context;

        public SetNotificationDecisionRequiredByDateHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<bool> HandleAsync(SetNotificationDecisionRequiredByDate message)
        {
            var assessment =
                await
                    context.NotificationAssessments.SingleAsync(
                        p => p.NotificationApplicationId == message.NotificationId);

            assessment.DecisionRequiredBy(message.DecisionRequiredByDate);

            await context.SaveChangesAsync();

            return true;
        }
    }
}