namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using System.Threading.Tasks;

    public interface IWasteTypeRepository
    {
        Task<WasteType> GetByNotificationId(Guid notificationId);

        void Add(WasteType wasteType);
    }
}