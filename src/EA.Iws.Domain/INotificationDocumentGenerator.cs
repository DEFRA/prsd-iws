namespace EA.Iws.Domain
{
    using System;
    using System.Threading.Tasks;

    public interface INotificationDocumentGenerator
    {
        Task<byte[]> GenerateNotificationDocument(Guid notificationId);
    }
}