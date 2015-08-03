namespace EA.Iws.RequestHandlers.Admin
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationAssessment;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.Admin;

    internal class SetDatesHandler : IRequestHandler<SetDates, Guid>
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;

        public SetDatesHandler(IwsContext context, IUserContext userContext)
        {
            this.context = context;
            this.userContext = userContext;
        }

        public async Task<Guid> HandleAsync(SetDates message)
        {
            var notification = context.NotificationAssessments.SingleOrDefault(a => a.NotificationApplicationId == message.NotificationApplicationId);
            
            if (notification == null)
            {
                var user = await context.Users.SingleAsync(u => u.Id == userContext.UserId.ToString());
                notification = new NotificationAssessment(message.NotificationApplicationId);
                context.NotificationAssessments.Add(notification);
            }

            notification.NotificationReceivedDate = message.NotificationReceivedDate;
            notification.PaymentReceivedDate = message.PaymentReceivedDate;
            notification.CommencementDate = message.CommencementDate;
            notification.CompleteDate = message.CompleteDate;
            notification.TransmittedDate = message.TransmittedDate;
            notification.AcknowledgedDate = message.AcknowledgedDate;
            notification.DecisionDate = message.DecisionDate;
            notification.NameOfOfficer = message.NameOfOfficer;

            await context.SaveChangesAsync();

            return notification.Id;
        }
    }
}