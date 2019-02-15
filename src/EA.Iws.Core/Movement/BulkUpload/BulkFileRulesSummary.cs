namespace EA.Iws.Core.Movement.BulkUpload
{
    using System.Collections.Generic;
    using System.Data;
    using Rules;

    public class BulkFileRulesSummary
    {
        public IEnumerable<RuleResult<BulkFileRules>> FileRulesResults { get; set; }

        public DataTable DataTable { get; set; }

        public byte[] FileBytes { get; set; }
    }
}
