namespace EA.Iws.Domain.ImportNotificationAssessment.Decision
{
    using System;
    using Prsd.Core.Domain;

    public class ImportObjection : Entity
    {
        public Guid NotificationId { get; private set; }

        public DateTime Date { get; private set; }

        public string Reasons { get; private set; }

        public ImportObjection(Guid notificationId, DateTime date, string reasons)
        {
            NotificationId = notificationId;
            Date = date;
            Reasons = reasons;
        }
    }
}
