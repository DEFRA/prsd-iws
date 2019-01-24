namespace EA.Iws.Core.Movement.Bulk
{
    using System.Collections.Generic;
    using System.Linq;
    using Rules;

    public class BulkMovementRulesSummary
    {
        public IEnumerable<RuleResult<BulkMovementFileRules>> FileRulesResults { get; private set; }
        public IEnumerable<ContentRuleResult<BulkMovementContentRules>> ContentRulesResults { get; private set; }

        public bool IsFileRulesSuccess
        {
            get { return FileRulesResults.All(r => r.MessageLevel == MessageLevel.Success); }
        }

        public bool IsContentRulesSuccess
        {
            get { return ContentRulesResults.All(r => r.MessageLevel == MessageLevel.Success); }
        }

        public BulkMovementRulesSummary(IEnumerable<RuleResult<BulkMovementFileRules>> fileRules, IEnumerable<ContentRuleResult<BulkMovementContentRules>> contentRules)
        {
            FileRulesResults = fileRules ?? new RuleResult<BulkMovementFileRules>[] { };
            ContentRulesResults = contentRules ?? new ContentRuleResult<BulkMovementContentRules>[] { }; 
        }
    }
}
