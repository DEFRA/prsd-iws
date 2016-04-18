namespace EA.Iws.RequestHandlers.Exporters
{
    using System.Threading.Tasks;
    using Core.Exporters;
    using Domain.NotificationApplication.Exporter;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Exporters;

    internal class GetExporterByNotificationIdHandler : IRequestHandler<GetExporterByNotificationId, ExporterData>
    {
        private readonly IExporterRepository exporterRepository;
        private readonly IMapWithParentObjectId<Exporter, ExporterData> mapper;

        public GetExporterByNotificationIdHandler(IExporterRepository exporterRepository, 
            IMapWithParentObjectId<Exporter, ExporterData> mapper)
        {
            this.exporterRepository = exporterRepository;
            this.mapper = mapper;
        }

        public async Task<ExporterData> HandleAsync(GetExporterByNotificationId message)
        {
            var notification = await exporterRepository.GetExporterOrDefaultByNotificationId(message.NotificationId);

            return mapper.Map(notification, message.NotificationId);
        }
    }
}