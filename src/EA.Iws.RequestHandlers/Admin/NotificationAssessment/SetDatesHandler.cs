namespace EA.Iws.RequestHandlers.Admin.NotificationAssessment
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationAssessment;
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

            var notificationDates = context.NotificationDates.SingleOrDefault(a => a.NotificationApplicationId == message.NotificationApplicationId);

            if (notificationDates == null)
            {
                notificationDates = new NotificationDates(message.NotificationApplicationId);
                context.NotificationDates.Add(notificationDates);
            }

            notificationDates.NotificationReceivedDate = message.NotificationReceivedDate;
            notificationDates.PaymentReceivedDate = message.PaymentReceivedDate;
            notificationDates.CommencementDate = message.CommencementDate;
            notificationDates.CompleteDate = message.CompleteDate;
            notificationDates.TransmittedDate = message.TransmittedDate;
            notificationDates.AcknowledgedDate = message.AcknowledgedDate;
            notificationDates.DecisionDate = message.DecisionDate;
            notificationDates.NameOfOfficer = message.NameOfOfficer;

            await context.SaveChangesAsync();

            return notificationDates.Id;
        }
    }
}