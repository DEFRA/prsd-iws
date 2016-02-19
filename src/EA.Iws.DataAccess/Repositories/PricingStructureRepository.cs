namespace EA.Iws.DataAccess.Repositories
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.Notification;
    using Core.Shared;
    using Domain.Finance;
    using Domain.NotificationApplication;

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

        public async Task<PricingStructure> GetExport(UKCompetentAuthority competentAuthority, NotificationType notificationType, int numberOfShipments, bool isInterim)
        {
            return await context.PricingStructures.SingleAsync(p =>
                p.CompetentAuthority == competentAuthority
                && p.Activity.TradeDirection == TradeDirection.Export
                && p.Activity.NotificationType == notificationType
                && p.Activity.IsInterim == isInterim
                && (p.ShipmentQuantityRange.RangeFrom <= numberOfShipments
                    && (p.ShipmentQuantityRange.RangeTo == null
                    || p.ShipmentQuantityRange.RangeTo >= numberOfShipments)));
        }
    }
}