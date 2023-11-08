namespace EA.Iws.Core.ImportNotification.Draft
{
    using System;
    using System.ComponentModel;
    using WasteComponentType;

    [DisplayName("Waste components")]
    public class WasteComponents : IDraftEntity
    {
        internal WasteComponents()
        {
        }

        public WasteComponents(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }

        public Guid ImportNotificationId { get; set; }

        public WasteComponentType[] WasteComponentTypes { get; set; }        
    }
}
