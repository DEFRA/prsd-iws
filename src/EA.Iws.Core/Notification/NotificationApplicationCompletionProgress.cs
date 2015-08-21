namespace EA.Iws.Core.Notification
{
    using System;
    using Shared;

    public class NotificationApplicationCompletionProgress
    {
        public Guid Id { get; set; }

        public string NotificationNumber { get; set; }

        public NotificationType NotificationType { get; set; }

        public CompetentAuthority CompetentAuthority { get; set; }

        public bool HasExporter { get; set; }
        public bool HasProducer { get; set; }
        public bool HasImporter { get; set; }
        public bool HasFacility { get; set; }

        public bool HasActualSiteOfTreatment { get; set; }
        public bool HasSiteOfExport { get; set; }

        public bool HasPreconsentedInformation { get; set; }
        public bool HasOperationCodes { get; set; }
        public bool HasTechnologyEmployed { get; set; }
        public bool HasReasonForExport { get; set; }

        public bool HasCarrier { get; set; }
        public bool HasMeansOfTransport { get; set; }
        public bool HasPackagingInfo { get; set; }
        public bool HasSpecialHandlingRequirements { get; set; }

        public bool HasStateOfExport { get; set; }
        public bool HasStateOfImport { get; set; }
        public bool HasTransitState { get; set; }
        public bool HasCustomsOffice { get; set; }

        public bool HasShipmentInfo { get; set; }

        public bool HasWasteType { get; set; }
        public bool HasWasteGenerationProcess { get; set; }
        public bool HasPhysicalCharacteristics { get; set; }

        public bool HasBaselOecdCode { get; set; }
        public bool HasEwcCodes { get; set; }
        public bool HasYCodes { get; set; }
        public bool HasHCodes { get; set; }
        public bool HasUnClasses { get; set; }
        public bool HasUnNumbers { get; set; }
        public bool HasOtherCodes { get; set; }

        public bool HasRecoveryData { get; set; }
        public bool HasRecoveryInfo { get; set; }

        public bool HasMethodOfDisposal { get; set; }

        public bool IsAllComplete { get; set; }
    }
}
