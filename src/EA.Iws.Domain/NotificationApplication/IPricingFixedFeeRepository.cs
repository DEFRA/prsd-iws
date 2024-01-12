namespace EA.Iws.Domain.NotificationApplication
{
    using EA.Iws.Core.WasteComponentType;
    using EA.Iws.Core.WasteType;
    using Finance;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPricingFixedFeeRepository
    {
        Task<IEnumerable<PricingFixedFee>> Get();

        Task<PricingFixedFee> GetWasteCategoryFee(WasteCategoryType wasteCategory, DateTimeOffset notificationSubmittedDate);

        Task<PricingFixedFee> GetWasteComponentFee(WasteComponentType wasteComponent, DateTimeOffset notificationSubmittedDate);

        Task<IEnumerable<PricingFixedFee>> GetAllWasteComponentFees(DateTimeOffset notificationSubmittedDate);
    }
}