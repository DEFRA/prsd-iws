namespace EA.Iws.Domain.NotificationApplication
{
    using System.Linq;
    using Core.CustomsOffice;

    public class NotificationProgress
    {
        private readonly NotificationApplication notification;

        public NotificationProgress(NotificationApplication notification)
        {
            this.notification = notification;
        }

        public bool IsExporterCompleted()
        {
            return notification.HasExporter;
        }

        public bool IsProducerCompleted()
        {
            return notification.Producers.Any(p => p.IsSiteOfExport);
        }

        public bool IsImporterCompleted()
        {
            return notification.HasImporter;
        }

        public bool IsFacilityCompleted()
        {
            return notification.Facilities.Any(f => f.IsActualSiteOfTreatment);
        }

        public bool IsPreconsentStatusChosen()
        {
            if (notification.NotificationType == NotificationType.Disposal)
            {
                return true;
            }

            return notification.IsPreconsentedRecoveryFacility.HasValue;
        }

        public bool AreOperationCodesChosen()
        {
            return notification.OperationInfos.Any();
        }

        public bool IsTechnologyEmployedCompleted()
        {
            return notification.TechnologyEmployed != null &&
                   (notification.TechnologyEmployed.AnnexProvided ||
                    !string.IsNullOrWhiteSpace(notification.TechnologyEmployed.Details));
        }

        public bool IsReasonForExportCompleted()
        {
            return !string.IsNullOrWhiteSpace(notification.ReasonForExport);
        }

        public bool IsCarrierCompleted()
        {
            return notification.Carriers.Any();
        }

        public bool IsMeansOfTransportCompleted()
        {
            return notification.MeansOfTransport.Any();
        }

        public bool IsPackagingTypesCompleted()
        {
            return notification.PackagingInfos.Any();
        }

        public bool IsSpecialHandlingCompleted()
        {
            if (notification.HasSpecialHandlingRequirements.HasValue &&
                notification.HasSpecialHandlingRequirements.Value)
            {
                return !string.IsNullOrWhiteSpace(notification.SpecialHandlingDetails);
            }

            return notification.HasSpecialHandlingRequirements.HasValue;
        }

        public bool IsStateOfExportCompleted()
        {
            return notification.StateOfExport != null;
        }

        public bool IsStateOfImportCompleted()
        {
            return notification.StateOfImport != null;
        }

        public bool IsCustomsOfficeCompleted()
        {
            if (IsStateOfExportCompleted() && IsStateOfImportCompleted())
            {
                var required = notification.GetCustomsOfficesRequired();
                var completed = notification.GetCustomsOfficesCompleted();

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
            return notification.TransitStates.Any();
        }

        public bool IsIntendedShipmentsCompleted()
        {
            return notification.ShipmentInfo != null;
        }

        public bool IsChemicalCompositionCompleted()
        {
            if (!notification.HasWasteType)
            {
                return false;
            }
            return (notification.WasteType.ChemicalCompositionType != ChemicalComposition.Other ||
                    (!string.IsNullOrWhiteSpace(notification.WasteType.OtherWasteTypeDescription)
                     || notification.WasteType.HasAnnex))
                   &&
                   (notification.WasteType.ChemicalCompositionType == ChemicalComposition.Other ||
                    notification.WasteType.WasteAdditionalInformation.Any())
                   &&
                   (notification.WasteType.ChemicalCompositionType != ChemicalComposition.Wood ||
                    !string.IsNullOrWhiteSpace(notification.WasteType.WoodTypeDescription))
                   &&
                   ((notification.WasteType.ChemicalCompositionType != ChemicalComposition.RDF &&
                     notification.WasteType.ChemicalCompositionType != ChemicalComposition.SRF) ||
                    !string.IsNullOrWhiteSpace(notification.WasteType.EnergyInformation));
        }

        public bool IsProcessOfGenerationCompleted()
        {
            return (notification.IsWasteGenerationProcessAttached.HasValue &&
                    notification.IsWasteGenerationProcessAttached.Value)
                   || !string.IsNullOrWhiteSpace(notification.WasteGenerationProcess);
        }

        public bool ArePhysicalCharacteristicsCompleted()
        {
            return notification.PhysicalCharacteristics.Any();
        }

        public bool AreWasteCodesCompleted()
        {
            return notification.BaselOecdCode != null && notification.ExportCode != null &&
                   notification.ImportCode != null
                   && notification.EwcCodes.Any() && notification.YCodes.Any() && notification.HCodes.Any() &&
                   notification.UnClasses.Any()
                   && notification.UnNumbers.Any();
        }

        public bool IsWasteRecoveryInformationCompleted()
        {
            if (notification.NotificationType == NotificationType.Disposal)
            {
                return true;
            }

            if (notification.IsProvidedByImporter.GetValueOrDefault())
            {
                return true;
            }

            if (notification.PercentageRecoverable.GetValueOrDefault() == 100.00M)
            {
                return true;
            }

            return (notification.PercentageRecoverable != null && notification.MethodOfDisposal != null);
        }

        public bool IsAllCompleted()
        {
            return AreOperationCodesChosen() && ArePhysicalCharacteristicsCompleted() &&
                   AreWasteCodesCompleted()
                   && IsCarrierCompleted() && IsChemicalCompositionCompleted() &&
                   IsCustomsOfficeCompleted()
                   && IsExporterCompleted() && IsFacilityCompleted() && IsImporterCompleted() &&
                   IsIntendedShipmentsCompleted()
                   && IsMeansOfTransportCompleted() && IsPackagingTypesCompleted() && IsPreconsentStatusChosen() &&
                   IsProcessOfGenerationCompleted()
                   && IsProducerCompleted() && IsReasonForExportCompleted() && IsSpecialHandlingCompleted() &&
                   IsStateOfExportCompleted()
                   && IsStateOfImportCompleted() && IsTechnologyEmployedCompleted() &&
                   IsWasteRecoveryInformationCompleted();
        }
    }
}