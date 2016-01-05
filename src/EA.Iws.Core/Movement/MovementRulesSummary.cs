namespace EA.Iws.Core.Movement
{
    using System.Collections.Generic;
    using System.Linq;
    using Rules;

    public class MovementRulesSummary
    {
        public MovementRulesSummary(IEnumerable<RuleResult<MovementRules>> ruleResults)
        {
            RuleResults = ruleResults ?? new RuleResult<MovementRules>[] { };
        }

        public IEnumerable<RuleResult<MovementRules>> RuleResults { get; private set; } 

        public bool IsSuccess
        {
            get { return RuleResults.All(r => r.MessageLevel == MessageLevel.Success); }
        }
    }
}