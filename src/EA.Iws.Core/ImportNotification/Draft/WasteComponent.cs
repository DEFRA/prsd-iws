namespace EA.Iws.Core.ImportNotification.Draft
{
    using System;
    using System.ComponentModel;
    using WasteComponentType;

    [DisplayName("Waste component")]
    public class WasteComponent : IDraftEntity
    {
        internal WasteComponent()
        {
        }

        public WasteComponent(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }

        public Guid ImportNotificationId { get; set; }

        public WasteComponentType[] WasteComponentTypes { get; set; }        
    }
}
