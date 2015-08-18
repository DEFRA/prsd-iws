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

    public class SetDecisionHandler : IRequestHandler<SetDecision, Guid>
    {
        private readonly IwsContext context;

        public SetDecisionHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(SetDecision message)
        {
            if (!await context.NotificationApplications.AnyAsync(p => p.Id == message.NotificationApplicationId))
            {
                throw new InvalidOperationException(string.Format("Notification {0} does not exist.", message.NotificationApplicationId));
            }

            var notificationDecision = context.NotificationDecisions.SingleOrDefault(a => a.NotificationApplicationId == message.NotificationApplicationId);
            if (notificationDecision == null)
            {
                notificationDecision = new NotificationDecision(message.NotificationApplicationId);
                context.NotificationDecisions.Add(notificationDecision);
            }

            notificationDecision.DecisionMade = message.DecisionMade;
            notificationDecision.ConsentedFrom = message.ConsentedFrom;
            notificationDecision.ConsentedTo = message.ConsentedTo;
            notificationDecision.DecisionType = message.DecisionType;
            notificationDecision.ConditionsOfConsent = message.ConditionsOfConsent;

            await context.SaveChangesAsync();

            return notificationDecision.Id;
        }
    }
}
