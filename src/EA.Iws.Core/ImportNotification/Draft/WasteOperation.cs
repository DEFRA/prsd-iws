namespace EA.Iws.Core.ImportNotification.Draft
{
    using System;
    using System.ComponentModel;
    using OperationCodes;

    [DisplayName("Waste operation")]
    public class WasteOperation : IDraftEntity
    {
        internal WasteOperation()
        {
        }

        public WasteOperation(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }

        public Guid ImportNotificationId { get; set; }

        public OperationCode[] OperationCodes { get; set; }

        public string TechnologyEmployed { get; set; }
    }
}