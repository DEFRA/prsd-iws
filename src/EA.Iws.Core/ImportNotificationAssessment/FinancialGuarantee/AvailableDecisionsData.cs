namespace EA.Iws.Core.ImportNotificationAssessment.FinancialGuarantee
{
    using System.Collections.Generic;
    using Admin;

    public class AvailableDecisionsData
    {
        public bool IsReceived { get; set; }

        public bool IsCompleted { get; set; }

        public IList<FinancialGuaranteeDecision> Decisions { get; set; }

        public ImportFinancialGuaranteeStatus Status { get; set; }

        public AvailableDecisionsData()
        {
            Status = ImportFinancialGuaranteeStatus.AwaitingApplication;
        }
    }
}
