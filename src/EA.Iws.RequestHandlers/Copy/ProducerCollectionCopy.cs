namespace EA.Iws.RequestHandlers.Copy
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using DataAccess;
    using Domain.NotificationApplication;

    [AutoRegister]
    public class ProducerCollectionCopy
    {
        public async Task CopyAsync(IwsContext context, Guid notificationSourceId, Guid notificationDestinationId)
        {
            var originalProducers = await context.Producers
                .AsNoTracking()
                .Include("ProducersCollection")
                .SingleOrDefaultAsync(p => p.NotificationId == notificationSourceId);

            var newProducers = new ProducerCollection(notificationDestinationId);

            if (originalProducers != null)
            {
                foreach (var producer in originalProducers.Producers)
                {
                    var newProducer = newProducers.AddProducer(producer.Business, producer.Address, producer.Contact);

                    if (producer.IsSiteOfExport)
                    {
                        typeof(Producer).GetProperty("IsSiteOfExport")
                            .SetValue(newProducer, true, null);
                    }
                }
            }

            context.Producers.Add(newProducers);
        }
    }
}