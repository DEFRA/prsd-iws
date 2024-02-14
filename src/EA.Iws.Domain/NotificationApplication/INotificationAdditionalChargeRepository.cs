namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface INotificationAdditionalChargeRepository
    {
        Task<IEnumerable<AdditionalCharge>> GetPagedNotificationAdditionalChargesById(Guid notificationId);
    }
}
