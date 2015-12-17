namespace EA.Iws.Domain.ImportNotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using Core.ImportNotificationAssessment;
    using Prsd.Core.Domain;
    using Prsd.Core.Extensions;

    public class ImportNotificationAssessment : Entity
    {
        public Guid NotificationApplicationId { get; private set; }

        public ImportNotificationStatus Status { get; private set; }

        public ImportNotificationDates Dates { get; private set; }

        protected virtual ICollection<ImportNotificationStatusChange> StatusChangeCollection { get; private set; }

        public IEnumerable<ImportNotificationStatusChange> StatusChanges
        {
            get { return StatusChangeCollection.ToSafeIEnumerable(); }
        }

        protected ImportNotificationAssessment()
        {
        }

        public ImportNotificationAssessment(Guid notificationId)
        {
            NotificationApplicationId = notificationId;
            Dates = new ImportNotificationDates();
            Status = ImportNotificationStatus.New;
        }
    }
}
