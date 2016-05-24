namespace EA.Iws.Domain.Security
{
    using System;
    using System.Threading.Tasks;

    public interface IImportNotificationApplicationAuthorization
    {
        Task EnsureAccessAsync(Guid notificationId);
    }
}