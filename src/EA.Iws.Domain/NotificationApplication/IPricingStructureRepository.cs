namespace EA.Iws.Domain.NotificationApplication
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Notification;
    using Core.Shared;
    using Finance;

    public interface IPricingStructureRepository
    {
        Task<IEnumerable<PricingStructure>> Get();

        Task<PricingStructure> GetExport(UKCompetentAuthority competentAuthority, NotificationType notificationType, int numberOfShipments, bool isInterim);
    }
}
