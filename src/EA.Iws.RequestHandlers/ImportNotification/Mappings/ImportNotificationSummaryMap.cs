namespace EA.Iws.RequestHandlers.ImportNotification.Mappings
{
    using System.Collections.Generic;
    using System.Linq;
    using Aspose.Words.Lists;
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
            bool? areFacilitiesPreconsented = null;
            if (source.Facilities != null)
            {
                areFacilitiesPreconsented = source.Facilities.AllFacilitiesPreconsented;
            }            

            return new Core.ImportNotificationSummary
            {
                Id = source.Notification.Id,
                Type = source.Notification.NotificationType,
                Number = source.Notification.NotificationNumber,
                Status = source.Assessment.Status,
                Exporter = mapper.Map<Core.Exporter>(source.Exporter),
                Importer = mapper.Map<Core.Importer>(source.Importer),
                Producer = mapper.Map<Core.Producer>(source.Producer),
                Facilities = source.Facilities != null ? source.Facilities.Facilities.Select(x => mapper.Map<Core.Facility>(x)).ToArray() : null,
                AreFacilitiesPreconsented = areFacilitiesPreconsented,
                IntendedShipment = mapper.Map<Core.IntendedShipment>(source.Shipment),
                WasteOperation = mapper.Map<Core.WasteOperation>(source.WasteOperation),
                WasteType = mapper.Map<Core.WasteType>(source.WasteType),
                TransitStates = source.TransportRoute != null ? source.TransportRoute.TransitStates.Select(x => mapper.Map<Core.TransitState>(x)).ToArray() : null,
                HasNoTransitStates = source.TransportRoute != null ? !source.TransportRoute.TransitStates.Any() : true,
                StateOfExport = source.TransportRoute != null ? mapper.Map<Core.StateOfExport>(source.TransportRoute.StateOfExport) : null,
                StateOfImport = source.TransportRoute != null ? mapper.Map<Core.StateOfImport>(source.TransportRoute.StateOfImport) : null,
                Composition = mapper.Map<Core.ChemicalComposition>(source.WasteType),
                //WasteComponents = mapper.Map<Core.WasteComponent>(source.WasteComponents)
            };
        }
    }
}