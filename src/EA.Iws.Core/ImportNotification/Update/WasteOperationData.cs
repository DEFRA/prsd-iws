namespace EA.Iws.Core.ImportNotification.Update
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using OperationCodes;

    [DisplayName("Waste operation")]
    public class WasteOperationData
    {
        internal WasteOperationData()
        {
        }

        public WasteOperationData(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }

        public Guid ImportNotificationId { get; set; }

        public IList<OperationCode> OperationCodes { get; set; }

        public string TechnologyEmployed { get; set; }

        public NotificationDetails Details { get; set; }
    }
}
