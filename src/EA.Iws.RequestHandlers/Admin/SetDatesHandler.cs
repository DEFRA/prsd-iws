namespace EA.Iws.RequestHandlers.Admin
{
    using System;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.Admin;

    internal class SetDatesHandler : IRequestHandler<SetDates, Guid>
    {
        private readonly IwsContext context;

        public SetDatesHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(SetDates message)
        {
            var notification = context.NotificationAssessments.SingleOrDefault(a => a.NotificationApplicationId == message.NotificationApplicationId);
            if (notification == null)
            {
                notification = new NotificationAssessment(message.NotificationApplicationId);
                context.NotificationAssessments.Add(notification);
            }

            notification.NotificationReceivedDate = message.NotificationReceivedDate;
            notification.PaymentRecievedDate = message.PaymentRecievedDate;
            notification.CommencementDate = message.CommencementDate;
            notification.CompleteDate = message.CompleteDate;
            notification.TransmittedDate = message.TransmittedDate;
            notification.AcknowledgedDate = message.AcknowledgedDate;
            notification.DecisionDate = message.DecisionDate;

            await context.SaveChangesAsync();

            return notification.Id;
        }
    }
}