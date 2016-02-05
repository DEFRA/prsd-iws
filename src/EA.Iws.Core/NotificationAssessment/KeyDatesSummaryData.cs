namespace EA.Iws.Core.NotificationAssessment
{
    using System.Collections.Generic;
    using Notification;

    public class KeyDatesSummaryData
    {
        public NotificationDatesData Dates { get; set; }

        public IList<FinancialGuaranteeDecisionData> FinancialGuaranteeDecisions { get; set; }

        public UKCompetentAuthority CompetentAuthority { get; set; }

        public bool IsLocalAreaSet { get; set; }

        public IList<NotificationAssessmentDecision> DecisionHistory { get; set; }

        public bool? IsInterim { get; set; }
    }
}
