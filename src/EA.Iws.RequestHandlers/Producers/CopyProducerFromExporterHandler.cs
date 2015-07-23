namespace EA.Iws.RequestHandlers.Producers
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.Producers;

    internal class CopyProducerFromExporterHandler : IRequestHandler<CopyProducerFromExporter, Guid>
    {
        private readonly IwsContext context;

        public CopyProducerFromExporterHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(CopyProducerFromExporter message)
        {
            var notification = await context.GetNotificationApplication(message.NotificationId);

            var exporterBusiness = notification.Exporter.Business;

            var business = ProducerBusiness.CreateProducerBusiness(exporterBusiness.Name,
                Enumeration.FromDisplayName<BusinessType>(exporterBusiness.Type), exporterBusiness.RegistrationNumber,
                exporterBusiness.OtherDescription);

            var producer = notification.AddProducer(business, notification.Exporter.Address,
                notification.Exporter.Contact);

            await context.SaveChangesAsync();

            return producer.Id;
        }
    }
}