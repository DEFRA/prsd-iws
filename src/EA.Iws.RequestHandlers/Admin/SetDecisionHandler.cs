namespace EA.Iws.RequestHandlers.Admin
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.Admin;

    public class SetDecisionHandler : IRequestHandler<SetDecision, Guid>
    {
        private readonly IwsContext context;

        public SetDecisionHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(SetDecision message)
        {
            var notification = context.NotificationAssessments.SingleOrDefault(a => a.NotificationApplicationId == message.NotificationApplicationId);
            if (notification == null)
            {
                notification = new NotificationAssessment(message.NotificationApplicationId);
                context.NotificationAssessments.Add(notification);
            }

            notification.DecisionMade = message.DecisionMade;
            notification.ConsentedFrom = message.ConsentedFrom;
            notification.ConsentedTo = message.ConsentedTo;
            notification.DecisionType = message.DecisionType;
            notification.ConditionsOfConsent = message.ConditionsOfConsent;

            await context.SaveChangesAsync();

            return notification.Id;
        }
    }
}
