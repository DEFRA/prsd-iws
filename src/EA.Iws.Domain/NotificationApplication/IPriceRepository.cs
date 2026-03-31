namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using System.Threading.Tasks;
    using Finance;

    public interface IPriceRepository
    {
        Task<PriceAndRefund> GetPriceAndRefundByNotificationId(Guid notificationId);

        Task<PriceAndRefund> GetDraftImportNotificationPrice(Guid notificationId);
    }
}