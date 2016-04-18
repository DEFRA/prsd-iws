namespace EA.Iws.RequestHandlers.Copy
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using DataAccess;
    using Domain.NotificationApplication;

    [AutoRegister]
    public class CarrierCollectionCopy
    {
        public async Task CopyAsync(IwsContext context, Guid notificationSourceId, Guid notificationDestinationId)
        {
            var originalCarriers = await context.Carriers
                .AsNoTracking()
                .Include("CarriersCollection")
                .SingleOrDefaultAsync(c => c.NotificationId == notificationSourceId);

            var newCarriers = new CarrierCollection(notificationDestinationId);

            if (originalCarriers != null)
            {
                foreach (var carrier in originalCarriers.Carriers)
                {
                    var newCarrier = newCarriers.AddCarrier(carrier.Business, carrier.Address, carrier.Contact);
                }
            }

            context.Carriers.Add(newCarriers);
        }
    }
}