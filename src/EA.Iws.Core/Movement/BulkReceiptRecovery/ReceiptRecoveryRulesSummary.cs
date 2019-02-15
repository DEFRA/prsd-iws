namespace EA.Iws.Core.Movement.BulkReceiptRecovery
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BulkUpload;
    using Rules;

    [Serializable]
    public class ReceiptRecoveryRulesSummary
    {
        public IEnumerable<RuleResult<BulkFileRules>> FileRulesResults { get; set; }

        public IEnumerable<ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>> ContentRulesResults { get; set; }

        public IEnumerable<int> ShipmentNumbers { get; set; }

        public Guid DraftBulkUploadId { get; set; }

        public bool IsFileRulesSuccess
        {
            get { return FileRulesResults.All(r => r.MessageLevel == MessageLevel.Success); }
        }

        public bool IsContentRulesSuccess
        {
            get { return ContentRulesResults.All(r => r.MessageLevel == MessageLevel.Success); }
        }

        public ReceiptRecoveryRulesSummary()
        {
            FileRulesResults = new List<RuleResult<BulkFileRules>>();
            ContentRulesResults = new List<ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>>();
        }

        public ReceiptRecoveryRulesSummary(IEnumerable<RuleResult<BulkFileRules>> fileRules)
        {
            FileRulesResults = fileRules ?? new List<RuleResult<BulkFileRules>>();
            ContentRulesResults = new List<ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>>(); 
        }
    }
}
