namespace EA.Iws.Core.Admin.Search
{
    using System;
    using System.Collections.Generic;
    using ImportNotificationAssessment;
    using NotificationAssessment;
    using OperationCodes;
    using Shared;

    public class AdvancedSearchCriteria
    {
        public string EwcCode { get; set; }

        public string ProducerName { get; set; }

        public string ImporterName { get; set; }

        public string ImportCountryName { get; set; }

        public Guid? LocalAreaId { get; set; }

        public DateTime? ConsentValidFromStart { get; set; }

        public DateTime? ConsentValidFromEnd { get; set; }

        public DateTime? ConsentValidToStart { get; set; }

        public DateTime? ConsentValidToEnd { get; set; }

        public string ExporterName { get; set; }

        public string BaselOecdCode { get; set; }

        public string FacilityName { get; set; }

        public string ExitPointName { get; set; }

        public string EntryPointName { get; set; }

        public DateTime? NotificationReceivedStart { get; set; }

        public DateTime? NotificationReceivedEnd { get; set; }

        public NotificationType? NotificationType { get; set; }

        public OperationCode[] OperationCodes { get; set; }

        public TradeDirection? TradeDirection { get; set; }

        public string ExportCountryName { get; set; }

        public NotificationStatus? NotificationStatus { get; set; }

        public ImportNotificationStatus? ImportNotificationStatus { get; set; }

        public bool? IsInterim { get; set; }

        public bool? BaselOecdCodeNotListed { get; set; }
    }
}