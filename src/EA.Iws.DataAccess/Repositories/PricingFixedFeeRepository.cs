namespace EA.Iws.DataAccess.Repositories
{
    using Domain.Finance;
    using Domain.NotificationApplication;
    using EA.Iws.Core.WasteComponentType;
    using EA.Iws.Core.WasteType;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    public class PricingFixedFeeRepository : IPricingFixedFeeRepository
    {
        private readonly IwsContext context;

        public PricingFixedFeeRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<PricingFixedFee>> Get()
        {
            return await context.PricingFixedFees.ToArrayAsync();
        }

        public async Task<IEnumerable<PricingFixedFee>> GetAllWasteComponentFees(DateTimeOffset notificationSubmittedDate)
        {
            return await context.PricingFixedFees
                .OrderByDescending(ps => ps.ValidFrom)
                .Where(p => p.WasteComponentTypeId != null
                    && p.ValidFrom <= notificationSubmittedDate)
                .ToArrayAsync();
        }

        public async Task<PricingFixedFee> GetWasteCategoryFee(WasteCategoryType wasteCategory, DateTimeOffset notificationSubmittedDate)
        {
            return await context.PricingFixedFees
                .OrderByDescending(ps => ps.ValidFrom)
                .Where(p => p.WasteCategoryTypeId == wasteCategory
                    && p.ValidFrom <= notificationSubmittedDate)
                .FirstOrDefaultAsync();
        }

        public async Task<PricingFixedFee> GetWasteComponentFee(WasteComponentType wasteComponent, DateTimeOffset notificationSubmittedDate)
        {
            return await context.PricingFixedFees
                .OrderByDescending(ps => ps.ValidFrom)
                .Where(p => p.WasteComponentTypeId == wasteComponent
                    && p.ValidFrom <= notificationSubmittedDate)
                .FirstOrDefaultAsync();
        }
    }
}