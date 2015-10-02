namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Mediator;

    public class ConsentNotificationApplication : IRequest<bool>
    {
        public Guid NotificationId { get; set; }

        public DateTime ConsentFrom { get; set; }

        public DateTime ConsentTo { get; set; }

        public string ConsentConditions { get; set; }

        public ConsentNotificationApplication(Guid notificationId, 
            DateTime consentFrom,
            DateTime consentTo,
            string consentConditions)
        {
            Guard.ArgumentNotNullOrEmpty(() => consentConditions, consentConditions);

            NotificationId = notificationId;
            ConsentFrom = consentFrom;
            ConsentTo = consentTo;
            ConsentConditions = consentConditions;
        }
    }
}
