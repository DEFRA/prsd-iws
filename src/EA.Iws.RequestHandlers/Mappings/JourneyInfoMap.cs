namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.Notification;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Requests.CustomsOffice;
    using Requests.Notification;
    using Requests.StateOfExport;

    internal class JourneyInfoMap : IMap<NotificationApplication, JourneyInfo>
    {
        private readonly IMap<NotificationApplication, NotificationApplicationCompletionProgress> completionProgressMapper;
        private readonly IMap<NotificationApplication, StateOfExportWithTransportRouteData> transportRouteMap;
        private readonly IMap<NotificationApplication, EntryCustomsOfficeAddData> customsOfficeEntryMap;
        private readonly IMap<NotificationApplication, ExitCustomsOfficeAddData> customsOfficeExitMap;

        public JourneyInfoMap(
            IMap<NotificationApplication, NotificationApplicationCompletionProgress> completionProgressMapper,
            IMap<NotificationApplication, StateOfExportWithTransportRouteData> transportRouteMap,
            IMap<NotificationApplication, EntryCustomsOfficeAddData> customsOfficeEntryMap,
        IMap<NotificationApplication, ExitCustomsOfficeAddData> customsOfficeExitMap)
        {
            this.completionProgressMapper = completionProgressMapper;
            this.transportRouteMap = transportRouteMap;
            this.customsOfficeEntryMap = customsOfficeEntryMap;
            this.customsOfficeExitMap = customsOfficeExitMap;
        }

        public JourneyInfo Map(NotificationApplication notification)
        {
            return new JourneyInfo
            {
                NotificationId = notification.Id,
                Progress = completionProgressMapper.Map(notification),
                TransportRoute = transportRouteMap.Map(notification),
                EntryCustomsOffice = customsOfficeEntryMap.Map(notification),
                ExitCustomsOffice = customsOfficeExitMap.Map(notification)
            };
        }
    }
}
