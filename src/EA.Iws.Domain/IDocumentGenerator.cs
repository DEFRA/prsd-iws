namespace EA.Iws.Domain
{
    using System;
    using System.Threading.Tasks;
    using Notification;

    public interface IDocumentGenerator
    {
        byte[] GenerateNotificationDocument(NotificationApplication notification);
    }
}