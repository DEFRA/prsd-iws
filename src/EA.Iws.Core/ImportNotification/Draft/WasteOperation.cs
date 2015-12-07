namespace EA.Iws.Core.ImportNotification.Draft
{
    using System;

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

        public int[] OperationCodes { get; set; }

        public string TechnologyEmployed { get; set; }
    }
}