namespace EA.Iws.Requests.Admin
{
    using System;
    using Prsd.Core.Mediator;

    public class SetDecision : IRequest<Guid>
    {
        public Guid NotificationApplicationId { get; set; }

        public DateTime? DecisionMade { get; set; }

        public DateTime? ConsentedFrom { get; set; }

        public DateTime? ConsentedTo { get; set; }

        public string ConditionsOfConsent { get; set; }

        public int DecisionType { get; set; }
    }
}
