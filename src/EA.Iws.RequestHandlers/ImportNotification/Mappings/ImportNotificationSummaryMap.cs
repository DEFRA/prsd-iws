namespace EA.Iws.RequestHandlers.ImportNotification.Mappings
{
    using System.Linq;
    using Domain.ImportNotification;
    using Prsd.Core.Mapper;
    using Core = Core.ImportNotification.Summary;

    internal class ImportNotificationSummaryMap : IMap<ImportNotificationOverview, Core.ImportNotificationSummary>
    {
        private readonly IMapper mapper;

        public ImportNotificationSummaryMap(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public Core.ImportNotificationSummary Map(ImportNotificationOverview source)
        {
            return new Core.ImportNotificationSummary
            {
                Id = source.Notification.Id,
                Type = source.Notification.NotificationType,
                Number = source.Notification.NotificationNumber,
                Status = source.Assessment.Status,
                Exporter = mapper.Map<Core.Exporter>(source.Exporter),
                Importer = mapper.Map<Core.Importer>(source.Importer),
                Producer = mapper.Map<Core.Producer>(source.Producer),
                Facilities = source.Facilities.Facilities.Select(x => mapper.Map<Core.Facility>(x)).ToArray(),
                AreFacilitiesPreconsented = source.Facilities.AllFacilitiesPreconsented,
                IntendedShipment = mapper.Map<Core.IntendedShipment>(source.Shipment),
                WasteOperation = mapper.Map<Core.WasteOperation>(source.WasteOperation),
                WasteType = mapper.Map<Core.WasteType>(source.WasteType),
                TransitStates = source.TransportRoute.TransitStates.Select(x => mapper.Map<Core.TransitState>(x)).ToArray(),
                HasNoTransitStates = !source.TransportRoute.TransitStates.Any(),
                StateOfExport = mapper.Map<Core.StateOfExport>(source.TransportRoute.StateOfExport),
                StateOfImport = mapper.Map<Core.StateOfImport>(source.TransportRoute.StateOfImport),
                Composition = mapper.Map<Core.ChemicalComposition>(source.WasteType)
            };
        }
    }
}