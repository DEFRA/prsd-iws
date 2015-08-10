namespace EA.Iws.Domain
{
    public interface INotificationDocumentGenerator
    {
        byte[] GenerateNotificationDocument(NotificationApplication.NotificationApplication notification);
    }
}