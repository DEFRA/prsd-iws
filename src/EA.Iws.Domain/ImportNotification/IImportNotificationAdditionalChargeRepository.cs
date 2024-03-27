namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IImportNotificationAdditionalChargeRepository
    {
        Task<IEnumerable<AdditionalCharge>> GetImportNotificationAdditionalChargesById(Guid notificationId);

        Task AddImportNotificationAdditionalCharge(AdditionalCharge additionalCharge);
    }
}
