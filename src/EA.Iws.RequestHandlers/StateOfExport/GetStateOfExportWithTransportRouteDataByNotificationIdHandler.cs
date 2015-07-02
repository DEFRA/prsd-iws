namespace EA.Iws.RequestHandlers.StateOfExport
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.StateOfExport;

    internal class GetStateOfExportWithTransportRouteDataByNotificationIdHandler : IRequestHandler<GetStateOfExportWithTransportRouteDataByNotificationId, StateOfExportWithTransportRouteData>
    {
        private readonly IwsContext context;
        private readonly IMap<NotificationApplication, StateOfExportWithTransportRouteData> transportRouteMap;

        public GetStateOfExportWithTransportRouteDataByNotificationIdHandler(IwsContext context,
            IMap<NotificationApplication, StateOfExportWithTransportRouteData> transportRouteMap)
        {
            this.context = context;
            this.transportRouteMap = transportRouteMap;
        }

        public async Task<StateOfExportWithTransportRouteData> HandleAsync(GetStateOfExportWithTransportRouteDataByNotificationId message)
        {
            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == message.Id);

            return transportRouteMap.Map(notification);
        }
    }
}
