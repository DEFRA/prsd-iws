namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using System.Threading.Tasks;
    using Core.Notification;

    public interface IProvidedAnnexesRepository
    {
        Task<ProvidedAnnexesData> GetProvidedAnnexesData(Guid notificationId);
    }
}
