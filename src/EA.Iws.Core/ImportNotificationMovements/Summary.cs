namespace EA.Iws.Core.ImportNotificationMovements
{
    using System;
    using System.Collections.Generic;
    using ImportMovement;
    using Shared;

    public class Summary
    {
        public string NotificationNumber { get; set; }

        public Guid Id { get; set; }

        public NotificationType NotificationType { get; set; }

        public IList<ImportMovementSummaryData> Movements { get; set; }
    }
}
