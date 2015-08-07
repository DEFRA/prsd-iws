namespace EA.Iws.Requests.Notification
{
    using System;
    using System.Collections.Generic;
    using Core.MeansOfTransport;
    using Core.Notification;
    using Core.OperationCodes;
    using Core.RecoveryInfo;
    using Core.Shared;
    using Core.Shipment;
    using Core.TechnologyEmployed;
    using Core.WasteCodes;
    using Core.WasteType;
    using CustomsOffice;
    using StateOfExport;

    public class NotificationInfo
    {
        public Guid NotificationId { get; set; }

        public CompetentAuthority CompetentAuthority { get; set; }

        public string NotificationNumber { get; set; }

        public string CompetentAuthorityName { get; set; }

        public OrganisationsInvolvedInfo OrganisationsInvolvedInfo { get; set; }

        public RecoveryOperationInfo RecoveryOperationInfo { get; set; }

        public TransportationInfo TransportationInfo { get; set; }

        public JourneyInfo JourneyInfo { get; set; }

        public ClassifyYourWasteInfo ClassifyYourWasteInfo { get; set; }

        public WasteRecoveryInfo WasteRecoveryInfo { get; set; }

        public AmountsAndDatesInfo AmountsAndDatesInfo { get; set; }

        public SubmitSummaryData SubmitSummaryData { get; set; }

        public int NotificationCharge { get; set; }
    }
}