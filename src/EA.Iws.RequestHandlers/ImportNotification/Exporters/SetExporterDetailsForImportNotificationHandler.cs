namespace EA.Iws.RequestHandlers.ImportNotification.Exporters
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportNotification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification.Exporters;

    internal class SetExporterDetailsForImportNotificationHandler : IRequestHandler<SetExporterDetailsForImportNotification, Unit>
    {
        private readonly IExporterRepository exporterRepository;
        private readonly ImportNotificationContext context;
        private readonly IMapper mapper;

        public SetExporterDetailsForImportNotificationHandler(IExporterRepository exporterRepository, ImportNotificationContext context, IMapper mapper)
        {
            this.exporterRepository = exporterRepository;
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Unit> HandleAsync(SetExporterDetailsForImportNotification message)
        {
            var exporter = await exporterRepository.GetByNotificationId(message.ImportNotificationId);
            var contact = mapper.Map<Contact>(message.ExporterDetails.Contact);
            exporter.UpdateContactAndName(contact, message.ExporterDetails.Name);

            await context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
