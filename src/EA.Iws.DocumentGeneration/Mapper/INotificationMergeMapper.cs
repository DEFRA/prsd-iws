namespace EA.Iws.DocumentGeneration.Mapper
{
    using Domain.Notification;

    public interface INotificationMergeMapper
    {
        string GetValueForMergeField(MergeField mergeField, NotificationApplication notification);
    }
}
