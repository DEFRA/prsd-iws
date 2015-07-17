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
            var notifiation = await context.GetNotificationApplication(message.NotificationId);

            var exporterBusiness = notifiation.Exporter.Business;

            var business = ProducerBusiness.CreateProducerBusiness(exporterBusiness.Name,
                Enumeration.FromDisplayName<BusinessType>(exporterBusiness.Type), exporterBusiness.RegistrationNumber,
                exporterBusiness.OtherDescription);

            var producer = notifiation.AddProducer(business, notifiation.Exporter.Address,
                notifiation.Exporter.Contact);

            await context.SaveChangesAsync();

            return producer.Id;
        }
    }
}