namespace EA.Iws.Core.ImportNotification.Draft
{
    using System;
    using System.ComponentModel;

    [DisplayName("Waste categories")]
    public class WasteCategories : IDraftEntity
    {
        public WasteCategories(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }        

        public Core.WasteType.WasteCategoryType? WasteCategoryType { get; set; }

        public Guid ImportNotificationId { get; private set; }
    }
}
