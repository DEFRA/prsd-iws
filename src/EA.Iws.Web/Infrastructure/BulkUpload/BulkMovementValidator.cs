namespace EA.Iws.Web.Infrastructure.BulkUpload
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using System.Web;
    using Core.Movement.Bulk;
    using Core.Rules;

    public class BulkMovementValidator : IBulkMovementValidator
    {
        private readonly IEnumerable<IBulkMovementPrenotificationFileRule> fileRules;

        public DataTable DataTable { get; set; }

        public BulkMovementValidator(IEnumerable<IBulkMovementPrenotificationFileRule> fileRules)
        {
            this.fileRules = fileRules;
        }

        public async Task<BulkMovementRulesSummary> GetValidationSummary(HttpPostedFileBase file)
        {
            var resultFileRules = await GetFileRules(file);

            return new BulkMovementRulesSummary(resultFileRules);
        }

        private async Task<List<RuleResult<BulkMovementFileRules>>> GetFileRules(HttpPostedFileBase file)
        {
            var rules = new List<RuleResult<BulkMovementFileRules>>();

            foreach (var rule in fileRules)
            {
                var result = await rule.GetResult(file);
                // Grab the DataTable from the File Parse rule.
                if (result.Rule == BulkMovementFileRules.FileParse)
                {
                    DataTable = rule.DataTable;
                }

                rules.Add(result);
            }
            
            return rules;
        }
    }
}