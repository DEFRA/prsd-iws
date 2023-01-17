namespace EA.Iws.Domain
{
    using System;
    using System.Threading.Tasks;

    public interface IArchiveNotificationRepository
    {
        Task<string> ArchiveNotificationAsync(Guid notificationId);
    }
}