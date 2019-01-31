namespace EA.Iws.Core.Movement.Bulk
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Rules;

    [Serializable]
    public class BulkMovementRulesSummary
    {
        public IEnumerable<RuleResult<BulkMovementFileRules>> FileRulesResults { get; set; }

        public IEnumerable<ContentRuleResult<BulkMovementContentRules>> ContentRulesResults { get; set; }

        public List<PrenotificationMovement> PrenotificationMovements { get; set; }
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

        public BulkMovementRulesSummary()
        {
            FileRulesResults = new List<RuleResult<BulkMovementFileRules>>();
            ContentRulesResults = new List<ContentRuleResult<BulkMovementContentRules>>();
        }

        public BulkMovementRulesSummary(IEnumerable<RuleResult<BulkMovementFileRules>> fileRules)
        {
            FileRulesResults = fileRules ?? new List<RuleResult<BulkMovementFileRules>>();
            ContentRulesResults = new List<ContentRuleResult<BulkMovementContentRules>>(); 
        }
    }
}
