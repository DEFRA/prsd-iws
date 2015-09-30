namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using System.Linq;
    using DataAccess;
    using Domain.NotificationApplication;
    using Domain.TransportRoute;
    using Prsd.Core.Mapper;
    using Requests.CustomsOffice;
    using Requests.Notification.Overview;
    using Requests.StateOfExport;

    internal class JourneyInfoMap : IMap<NotificationApplication, Journey>
    {
        private readonly IwsContext context;
        private readonly IMapWithParameter<TransportRoute, Guid, StateOfExportWithTransportRouteData> transportRouteMap;
        private readonly IMap<TransportRoute, EntryCustomsOfficeAddData> customsOfficeEntryMap;
        private readonly IMap<TransportRoute, ExitCustomsOfficeAddData> customsOfficeExitMap;

        public JourneyInfoMap(
            IwsContext context,
            IMapWithParameter<TransportRoute, Guid, StateOfExportWithTransportRouteData> transportRouteMap,
            IMap<TransportRoute, EntryCustomsOfficeAddData> customsOfficeEntryMap,
            IMap<TransportRoute, ExitCustomsOfficeAddData> customsOfficeExitMap)
        {
            this.context = context;
            this.transportRouteMap = transportRouteMap;
            this.customsOfficeEntryMap = customsOfficeEntryMap;
            this.customsOfficeExitMap = customsOfficeExitMap;
        }

        public Journey Map(NotificationApplication notification)
        {
            var transportRoute = context.TransportRoutes.SingleOrDefault(p => p.NotificationId == notification.Id);

            return new Journey
            {
                NotificationId = notification.Id,
                TransportRoute = transportRouteMap.Map(transportRoute, notification.Id),
                EntryCustomsOffice = customsOfficeEntryMap.Map(transportRoute),
                ExitCustomsOffice = customsOfficeExitMap.Map(transportRoute)
            };
        }
    }
}
