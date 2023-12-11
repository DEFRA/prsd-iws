namespace EA.Iws.DataAccess.Repositories
{
    using Core.Notification;
    using Core.Shared;
    using Domain.Finance;
    using Domain.NotificationApplication;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    public class PricingStructureRepository : IPricingStructureRepository
    {
        private readonly IwsContext context;

        public PricingStructureRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<PricingStructure>> Get()
        {
            return await context.PricingStructures.ToArrayAsync();
        }

        public async Task<PricingStructure> GetExport(UKCompetentAuthority competentAuthority, NotificationType notificationType, int numberOfShipments, bool isInterim, DateTimeOffset notificationSubmittedDate)
        {
            return await context.PricingStructures
                .OrderByDescending(ps => ps.ValidFrom)
                .Where(p => p.CompetentAuthority == competentAuthority
                    && p.ValidFrom <= notificationSubmittedDate
                    && p.Activity.TradeDirection == TradeDirection.Export
                    && p.Activity.NotificationType == notificationType
                    && p.Activity.IsInterim == isInterim
                    && (p.ShipmentQuantityRange.RangeFrom <= numberOfShipments
                        && (p.ShipmentQuantityRange.RangeTo == null
                        || p.ShipmentQuantityRange.RangeTo >= numberOfShipments)))
                .FirstOrDefaultAsync();
        }
    }
}