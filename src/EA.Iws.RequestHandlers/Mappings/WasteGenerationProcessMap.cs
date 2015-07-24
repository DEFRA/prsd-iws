namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.WasteType;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;

    internal class WasteGenerationProcessMap : IMap<NotificationApplication, WasteGenerationProcessData>
    {
        public WasteGenerationProcessData Map(NotificationApplication source)
        {
            return new WasteGenerationProcessData
            {
                NotificationId = source.Id,
                IsDocumentAttached = source.IsWasteGenerationProcessAttached.GetValueOrDefault(),
                Process = source.WasteGenerationProcess
            };
        }
    }
}