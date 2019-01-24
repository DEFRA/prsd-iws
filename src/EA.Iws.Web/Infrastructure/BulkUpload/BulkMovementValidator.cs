namespace EA.Iws.Web.Infrastructure.BulkUpload
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using System.Web;
    using Core.Movement.Bulk;
    using Core.Rules;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using VirusScanning;

    public class BulkMovementValidator : IBulkMovementValidator
    {
        private readonly IMediator mediator;
        private readonly IEnumerable<IBulkMovementPrenotificationFileRule> fileRules;

        public DataTable DataTable { get; set; }

        public BulkMovementValidator(IMediator mediator, IEnumerable<IBulkMovementPrenotificationFileRule> fileRules)
        {
            this.mediator = mediator;
            this.fileRules = fileRules;
        }

        public async Task<BulkMovementRulesSummary> GetValidationSummary(HttpPostedFileBase file)
        {
            var resultFileRules = await GetFileRules(file);

            var bulkMovementRulesSummary = new BulkMovementRulesSummary(resultFileRules);
            // COULLM: Suspect we need a way identify, and thus prevent GetContentRules from being run if we have at least one error in the FileRules
            bulkMovementRulesSummary = await mediator.SendAsync(new PerformBulkUploadContentValidation(bulkMovementRulesSummary));
            
            return bulkMovementRulesSummary;
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