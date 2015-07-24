namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.Notification;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;

    public class NotificationApplicationCompletionProgressMap : IMap<NotificationApplication, NotificationApplicationCompletionProgress>
    {
        public NotificationApplicationCompletionProgress Map(NotificationApplication source)
        {
            return new NotificationApplicationCompletionProgress
            {
                AreOperationCodesChosen = source.AreOperationCodesChosen(),
                ArePhysicalCharacteristicsCompleted = source.ArePhysicalCharacteristicsCompleted(),
                AreTransitStatesCompleted = source.AreTransitStatesCompleted(),
                AreWasteCodesCompleted = source.AreWasteCodesCompleted(),
                IsCarrierCompleted = source.IsCarrierCompleted(),
                IsChemicalCompositionCompleted = source.IsChemicalCompositionCompleted(),
                IsCustomsOfficeCompleted = source.IsCustomsOfficeCompleted(),
                IsExporterCompleted = source.IsExporterCompleted(),
                IsFacilityCompleted = source.IsFacilityCompleted(),
                IsImporterCompleted = source.IsImporterCompleted(),
                IsIntendedShipmentsCompleted = source.IsIntendedShipmentsCompleted(),
                IsMeansOfTransportCompleted = source.IsMeansOfTransportCompleted(),
                IsPackagingTypesCompleted = source.IsPackagingTypesCompleted(),
                IsPreconsentStatusChosen = source.IsPreconsentStatusChosen(),
                IsProcessOfGenerationCompleted = source.IsProcessOfGenerationCompleted(),
                IsProducerCompleted = source.IsProducerCompleted(),
                IsReasonForExportCompleted = source.IsReasonForExportCompleted(),
                IsSpecialHandlingCompleted = source.IsSpecialHandlingCompleted(),
                IsStateOfExportCompleted = source.IsStateOfExportCompleted(),
                IsStateOfImportCompleted = source.IsStateOfImportCompleted(),
                IsTechnologyEmployedCompleted = source.IsTechnologyEmployedCompleted(),
                IsWasteRecoveryInformationCompleted = source.IsWasteRecoveryInformationCompleted()
            };
        }
    }
}
