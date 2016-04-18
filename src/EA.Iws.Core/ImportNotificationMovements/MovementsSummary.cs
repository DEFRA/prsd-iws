namespace EA.Iws.Core.ImportNotificationMovements
{
    using System;
    using System.Collections.Generic;
    using Shared;

    public class MovementsSummary
    {
        public Guid ImportNotificationId { get; set; }

        public NotificationType NotificationType { get; set; }

        public List<MovementTableData> TableData { get; set; }
    }
}
