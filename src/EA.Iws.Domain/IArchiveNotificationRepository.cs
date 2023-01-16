namespace EA.Iws.Domain
{
    using EA.Iws.Core.Admin.ArchiveNotification;
    using System;
    using System.Threading.Tasks;

    public interface IArchiveNotificationRepository
    {
        Task<ArchiveNotificationResult> ArchiveNotificationAsync(Guid notificationId);
    }
}