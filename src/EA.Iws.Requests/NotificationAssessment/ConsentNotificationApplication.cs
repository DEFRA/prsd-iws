namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.Shared;
    using Prsd.Core;
    using Prsd.Core.Mediator;

    public class ConsentNotificationApplication : IRequest<bool>
    {
        public Guid NotificationId { get; set; }

        public DateRange ConsentRange { get; set; }

        public string ConsentConditions { get; set; }

        public ConsentNotificationApplication(Guid notificationId, 
            DateRange consentRange,
            string consentConditions)
        {
            Guard.ArgumentNotNullOrEmpty(() => consentConditions, consentConditions);

            NotificationId = notificationId;
            ConsentRange = consentRange;
            ConsentConditions = consentConditions;
        }
    }
}
