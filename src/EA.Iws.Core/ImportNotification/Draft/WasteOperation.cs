namespace EA.Iws.Core.ImportNotification.Draft
{
    using System;

    public class WasteOperation
    {
        protected WasteOperation()
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