namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using Core.Shared;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class Consent : Entity
    {
        public DateTime ValidFrom { get; private set; }

        public DateTime ValidTo { get; private set; }

        public string Conditions { get; private set; }

        public Guid UserId { get; private set; }

        public Consent(DateRange consentRange, string conditions, Guid userId)
        {
            Guard.ArgumentNotNullOrEmpty(() => conditions, conditions);

            ValidFrom = consentRange.From;
            ValidTo = consentRange.To;

            Conditions = conditions;

            UserId = userId;
        }
    }
}
