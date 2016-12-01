namespace EA.Iws.Domain.ImportNotificationAssessment
{
    using System;
    using System.Threading.Tasks;

    public interface IImportNotificationChargeCalculator
    {
        Task<decimal> GetValue(Guid importNotificationId);

        Task<decimal> GetValueForNumberOfShipments(Guid notificationId, int numberOfShipments);
    }
}
