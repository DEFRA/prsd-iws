namespace EA.Iws.RequestHandlers.Exporters
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.Exporters;
    using DataAccess;
    using Domain.Notification;
    using Mappings;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Exporters;

    internal class GetExporterByNotificationIdHandler : IRequestHandler<GetExporterByNotificationId, ExporterData>
    {
        private readonly IwsContext context;
        private readonly IMap<NotificationApplication, ExporterData> mapper; 

        public GetExporterByNotificationIdHandler(IwsContext context, IMap<NotificationApplication, ExporterData> mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<ExporterData> HandleAsync(GetExporterByNotificationId message)
        {
            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == message.NotificationId);

            return mapper.Map(notification);
        }
    }
}