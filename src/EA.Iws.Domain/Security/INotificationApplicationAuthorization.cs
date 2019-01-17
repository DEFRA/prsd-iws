namespace EA.Iws.Domain.Security
{
    using System;
    using System.Threading.Tasks;

    public interface INotificationApplicationAuthorization
    {
        Task EnsureAccessAsync(Guid notificationId);

        Task EnsureAccessIsOwnerAsync(Guid notificationId);
    }
}