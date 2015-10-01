namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using Prsd.Core;

    public class ConsentDecision
    {
        public DateTime ValidFrom { get; private set; }

        public DateTime ValidTo { get; private set; }

        public string Conditions { get; private set; }

        public DateTime DecisionDate { get; private set; }

        public ConsentDecision(DateRange consentRange, string conditions, DateTime decisionDate)
        {
            Guard.ArgumentNotNullOrEmpty(() => conditions, conditions);

            ValidFrom = consentRange.From;
            ValidTo = consentRange.To;

            Conditions = conditions;

            DecisionDate = decisionDate;
        }
    }
}
