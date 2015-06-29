namespace EA.Iws.Domain.Notification
{
    using System.Linq;
    using Core.CustomsOffice;

    public partial class NotificationApplication
    {
        public bool IsExporterCompleted()
        {
            return HasExporter;
        }

        public bool IsProducerCompleted()
        {
            return Producers.Any(p => p.IsSiteOfExport);
        }

        public bool IsImporterCompleted()
        {
            return HasImporter;
        }

        public bool IsFacilityCompleted()
        {
            return Facilities.Any(f => f.IsActualSiteOfTreatment);
        }

        public bool IsPreconsentStatusChosen()
        {
            if (NotificationType == NotificationType.Disposal)
            {
                return true;
            }

            return IsPreconsentedRecoveryFacility.HasValue;
        }

        public bool AreOperationCodesChosen()
        {
            return OperationInfos.Any();
        }

        public bool IsTechnologyEmployedCompleted()
        {
            return TechnologyEmployed != null &&
                   (TechnologyEmployed.AnnexProvided || !string.IsNullOrWhiteSpace(TechnologyEmployed.Details));
        }

        public bool IsReasonForExportCompleted()
        {
            return !string.IsNullOrWhiteSpace(ReasonForExport);
        }

        public bool IsCarrierCompleted()
        {
            return Carriers.Any();
        }

        public bool IsMeansOfTransportCompleted()
        {
            return !string.IsNullOrWhiteSpace(MeansOfTransportInternal);
        }

        public bool IsPackagingTypesCompleted()
        {
            return PackagingInfos.Any();
        }

        public bool IsSpecialHandlingCompleted()
        {
            if (HasSpecialHandlingRequirements.HasValue && HasSpecialHandlingRequirements.Value)
            {
                return !string.IsNullOrWhiteSpace(SpecialHandlingDetails);
            }

            return HasSpecialHandlingRequirements.HasValue;
        }

        public bool IsStateOfExportCompleted()
        {
            return StateOfExport != null;
        }

        public bool IsStateOfImportCompleted()
        {
            return StateOfImport != null;
        }

        public bool IsCustomsOfficeCompleted()
        {
            if (IsStateOfExportCompleted() && IsStateOfImportCompleted())
            {
                var required = GetCustomsOfficesRequired();
                var completed = GetCustomsOfficesCompleted();

                switch (required)
                {
                    case CustomsOffices.None:
                        return true;
                    case CustomsOffices.Entry:
                        return completed == CustomsOffices.Entry || completed == CustomsOffices.EntryAndExit;
                    case CustomsOffices.Exit:
                        return completed == CustomsOffices.Exit || completed == CustomsOffices.EntryAndExit;
                    case CustomsOffices.EntryAndExit:
                        return completed == CustomsOffices.EntryAndExit;
                }
            }

            return false;
        }

        public bool AreTransitStatesCompleted()
        {
            if (TransitStates.Any())
            {
                return true;
            }

            return IsStateOfExportCompleted() && IsStateOfImportCompleted() && IsCustomsOfficeCompleted();
        }

        public bool IsIntendedShipmentsCompleted()
        {
            return ShipmentInfo != null;
        }

        public bool IsChemicalCompositionCompleted()
        {
            return HasWasteType;
        }

        public bool IsProcessOfGenerationCompleted()
        {
            return (IsWasteGenerationProcessAttached.HasValue && IsWasteGenerationProcessAttached.Value) 
                || !string.IsNullOrWhiteSpace(WasteGenerationProcess);
        }

        public bool ArePhysicalCharacteristicsCompleted()
        {
            return PhysicalCharacteristics.Any();
        }

        public bool AreWasteCodesCompleted()
        {
            return WasteCodeInfo.Any();
        }

        public bool IsWasteRecoveryInformationCompleted()
        {
            return RecoveryInfo != null;
        }
    }
}
