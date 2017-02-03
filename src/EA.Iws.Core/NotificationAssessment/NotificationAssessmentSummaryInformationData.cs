namespace EA.Iws.Core.NotificationAssessment
{
    using System;
    using Notification;

    public class NotificationAssessmentSummaryInformationData
    {
        public Guid Id { get; set; }

        public string Number { get; set; }

        public NotificationStatus Status { get; set; }

        public UKCompetentAuthority CompetentAuthority { get; set; }

        public string Area { get; set; }

        public bool? IsInterim { get; set; }

        public bool? AllFacilitiesPreconsented { get; set; }
    }
}
