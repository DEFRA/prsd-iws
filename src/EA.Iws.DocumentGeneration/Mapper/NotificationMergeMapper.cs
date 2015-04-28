namespace EA.Iws.DocumentGeneration.Mapper
{
    using System;
    using Domain.Notification;

    class NotificationMergeMapper : INotificationMergeMapper
    {
        public string GetValueForMergeField(MergeField mergeField, NotificationApplication notification)
        {
            if (mergeField.FieldName.InnerTypeName.Equals("Number"))
            {
                return notification.NotificationNumber;
            }

            if (notification.WasteAction == WasteAction.Disposal)
            {
                if (mergeField.FieldName.InnerTypeName.Equals("IsDisposal")) return "☑";

                if (mergeField.FieldName.InnerTypeName.Equals("IsRecovery")) return "☐";
            }

            if (notification.WasteAction == WasteAction.Recovery)
            {
                if (mergeField.FieldName.InnerTypeName.Equals("IsRecovery")) return "☑";

                if (mergeField.FieldName.InnerTypeName.Equals("IsDisposal")) return "☐";
            }

            return String.Empty;
        }
    }
}
