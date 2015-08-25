namespace EA.Iws.RequestHandlers.Admin.NotificationAssessment
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Admin.NotificationAssessment;

    internal class SetDatesHandler : IRequestHandler<SetDates, Guid>
    {
        private readonly IwsContext context;

        public SetDatesHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(SetDates message)
        {
            if (!await context.NotificationApplications.AnyAsync(p => p.Id == message.NotificationApplicationId))
            {
                throw new InvalidOperationException(string.Format("Notification {0} does not exist.", message.NotificationApplicationId));
            }

            var notificationDates = await context.NotificationAssessments.Where(a => a.NotificationApplicationId == message.NotificationApplicationId).Select(p => p.Dates).SingleAsync();

            notificationDates.AcknowledgedDate = message.AcknowledgedDate;
            notificationDates.DecisionDate = message.DecisionDate;

            await context.SaveChangesAsync();

            return notificationDates.Id;
        }
    }
}