namespace EA.Iws.Core.Movement.BulkPrenotification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Rules;

    [Serializable]
    public class PrenotificationRulesSummary
    {
        public IEnumerable<RuleResult<PrenotificationFileRules>> FileRulesResults { get; set; }

        public IEnumerable<PrenotificationContentRuleResult<PrenotificationContentRules>> ContentRulesResults { get; set; }

        public IEnumerable<int> ShipmentNumbers { get; set; }

        public Guid DraftBulkUploadId { get; set; }

        public bool RemovedFirstRow { get; set; }

        public byte[] FileBytes { get; set; }

        public bool IsFileRulesSuccess
        {
            get { return FileRulesResults.All(r => r.MessageLevel == MessageLevel.Success); }
        }

        public bool IsContentRulesSuccess
        {
            get { return ContentRulesResults.All(r => r.MessageLevel == MessageLevel.Success); }
        }

        public PrenotificationRulesSummary()
        {
            FileRulesResults = new List<RuleResult<PrenotificationFileRules>>();
            ContentRulesResults = new List<PrenotificationContentRuleResult<PrenotificationContentRules>>();
        }

        public PrenotificationRulesSummary(IEnumerable<RuleResult<PrenotificationFileRules>> fileRules)
        {
            FileRulesResults = fileRules ?? new List<RuleResult<PrenotificationFileRules>>();
            ContentRulesResults = new List<PrenotificationContentRuleResult<PrenotificationContentRules>>(); 
        }
    }
}
