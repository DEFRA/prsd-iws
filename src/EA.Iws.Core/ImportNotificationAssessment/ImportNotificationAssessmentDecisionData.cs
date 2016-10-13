namespace EA.Iws.Core.ImportNotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using Admin;

    public class ImportNotificationAssessmentDecisionData
    {
        public ImportNotificationStatus Status { get; set; }

        public IList<DecisionType> AvailableDecisions { get; set; }

        public DateTime AcknowledgedOnDate { get; set; }

        public bool IsPreconsented { get; set; }
    }
}
