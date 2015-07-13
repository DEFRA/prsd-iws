namespace EA.Iws.Core.Notification
{
    public class NotificationApplicationCompletionProgress
    {
        public bool IsExporterCompleted { get; set; }
        public bool IsProducerCompleted { get; set; }
        public bool IsImporterCompleted { get; set; }
        public bool IsFacilityCompleted { get; set; }

        public bool IsPreconsentStatusChosen { get; set; }
        public bool AreOperationCodesChosen { get; set; }
        public bool IsTechnologyEmployedCompleted { get; set; }
        public bool IsReasonForExportCompleted { get; set; }

        public bool IsCarrierCompleted { get; set; }
        public bool IsMeansOfTransportCompleted { get; set; }
        public bool IsPackagingTypesCompleted { get; set; }
        public bool IsSpecialHandlingCompleted { get; set; }

        public bool IsStateOfExportCompleted { get; set; }
        public bool IsStateOfImportCompleted { get; set; }
        public bool AreTransitStatesCompleted { get; set; }
        public bool IsCustomsOfficeCompleted { get; set; }

        public bool IsIntendedShipmentsCompleted { get; set; }

        public bool IsChemicalCompositionCompleted { get; set; }
        public bool IsProcessOfGenerationCompleted { get; set; }
        public bool ArePhysicalCharacteristicsCompleted { get; set; }
        public bool AreWasteCodesCompleted { get; set; }

        public bool IsWasteRecoveryInformationCompleted { get; set; }

        public bool IsAllComplete
        {
            get
            {
                return IsExporterCompleted && IsProducerCompleted && IsImporterCompleted && IsFacilityCompleted
                       && IsPreconsentStatusChosen && AreOperationCodesChosen && IsTechnologyEmployedCompleted
                       && IsReasonForExportCompleted && IsCarrierCompleted && IsMeansOfTransportCompleted
                       && IsPackagingTypesCompleted && IsSpecialHandlingCompleted && IsStateOfExportCompleted
                       && IsStateOfImportCompleted && IsCustomsOfficeCompleted
                       && IsIntendedShipmentsCompleted && IsChemicalCompositionCompleted &&
                       IsProcessOfGenerationCompleted
                       && ArePhysicalCharacteristicsCompleted && AreWasteCodesCompleted &&
                       IsWasteRecoveryInformationCompleted;
            }
        }
    }
}
