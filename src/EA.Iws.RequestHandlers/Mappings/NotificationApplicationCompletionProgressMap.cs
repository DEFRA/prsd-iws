namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.Notification;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;

    public class NotificationApplicationCompletionProgressMap : IMap<NotificationApplication, NotificationApplicationCompletionProgress>
    {
        public NotificationApplicationCompletionProgress Map(NotificationApplication source)
        {
            var progress = new NotificationProgress(source);

            return new NotificationApplicationCompletionProgress
            {
                AreOperationCodesChosen = progress.AreOperationCodesChosen(),
                ArePhysicalCharacteristicsCompleted = progress.ArePhysicalCharacteristicsCompleted(),
                AreTransitStatesCompleted = progress.AreTransitStatesCompleted(),
                AreWasteCodesCompleted = progress.AreWasteCodesCompleted(),
                IsCarrierCompleted = progress.IsCarrierCompleted(),
                IsChemicalCompositionCompleted = progress.IsChemicalCompositionCompleted(),
                IsCustomsOfficeCompleted = progress.IsCustomsOfficeCompleted(),
                IsExporterCompleted = progress.IsExporterCompleted(),
                IsFacilityCompleted = progress.IsFacilityCompleted(),
                IsImporterCompleted = progress.IsImporterCompleted(),
                IsIntendedShipmentsCompleted = progress.IsIntendedShipmentsCompleted(),
                IsMeansOfTransportCompleted = progress.IsMeansOfTransportCompleted(),
                IsPackagingTypesCompleted = progress.IsPackagingTypesCompleted(),
                IsPreconsentStatusChosen = progress.IsPreconsentStatusChosen(),
                IsProcessOfGenerationCompleted = progress.IsProcessOfGenerationCompleted(),
                IsProducerCompleted = progress.IsProducerCompleted(),
                IsReasonForExportCompleted = progress.IsReasonForExportCompleted(),
                IsSpecialHandlingCompleted = progress.IsSpecialHandlingCompleted(),
                IsStateOfExportCompleted = progress.IsStateOfExportCompleted(),
                IsStateOfImportCompleted = progress.IsStateOfImportCompleted(),
                IsTechnologyEmployedCompleted = progress.IsTechnologyEmployedCompleted(),
                IsWasteRecoveryInformationCompleted = progress.IsWasteRecoveryInformationCompleted(),
                IsAllComplete = progress.IsAllCompleted()
            };
        }
    }
}
