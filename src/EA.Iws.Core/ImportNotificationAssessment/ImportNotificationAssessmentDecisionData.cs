namespace EA.Iws.Core.ImportNotificationAssessment
{
    using System.Collections.Generic;
    using Admin;

    public class ImportNotificationAssessmentDecisionData
    {
        public ImportNotificationStatus Status { get; set; }

        public IList<DecisionType> AvailableDecisions { get; set; }
    }
}
