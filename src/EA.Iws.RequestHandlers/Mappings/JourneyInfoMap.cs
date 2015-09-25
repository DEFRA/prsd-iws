namespace EA.Iws.RequestHandlers.Mappings
{
    using System.Linq;
    using DataAccess;
    using Domain.NotificationApplication;
    using Domain.TransportRoute;
    using Prsd.Core.Mapper;
    using Requests.CustomsOffice;
    using Requests.Notification;
    using Requests.StateOfExport;

    internal class JourneyInfoMap : IMap<NotificationApplication, JourneyInfo>
    {
        private readonly IwsContext context;
        private readonly IMap<TransportRoute, StateOfExportWithTransportRouteData> transportRouteMap;
        private readonly IMap<TransportRoute, EntryCustomsOfficeAddData> customsOfficeEntryMap;
        private readonly IMap<TransportRoute, ExitCustomsOfficeAddData> customsOfficeExitMap;

        public JourneyInfoMap(
            IwsContext context,
            IMap<TransportRoute, StateOfExportWithTransportRouteData> transportRouteMap,
            IMap<TransportRoute, EntryCustomsOfficeAddData> customsOfficeEntryMap,
            IMap<TransportRoute, ExitCustomsOfficeAddData> customsOfficeExitMap)
        {
            this.context = context;
            this.transportRouteMap = transportRouteMap;
            this.customsOfficeEntryMap = customsOfficeEntryMap;
            this.customsOfficeExitMap = customsOfficeExitMap;
        }

        public JourneyInfo Map(NotificationApplication notification)
        {
            var transportRoute = context.TransportRoutes.SingleOrDefault(p => p.NotificationId == notification.Id);

            return new JourneyInfo
            {
                NotificationId = notification.Id,
                TransportRoute = transportRouteMap.Map(transportRoute),
                EntryCustomsOffice = customsOfficeEntryMap.Map(transportRoute),
                ExitCustomsOffice = customsOfficeExitMap.Map(transportRoute)
            };
        }
    }
}
