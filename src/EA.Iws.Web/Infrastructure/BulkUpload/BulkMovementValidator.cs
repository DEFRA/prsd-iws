namespace EA.Iws.Web.Infrastructure.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using Core.Documents;
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

        public async Task<BulkMovementRulesSummary> GetPrenotificationValidationSummary(HttpPostedFileBase file, Guid notificationId)
        {
            var resultFileRules = await GetFileRules(file, FileUploadType.Prenotification);

            var extension = Path.GetExtension(file.FileName);
            var isCsv = extension == ".csv" ? true : false;

            var bulkMovementRulesSummary = new BulkMovementRulesSummary(resultFileRules);
            if (bulkMovementRulesSummary.IsFileRulesSuccess)
            {
                bulkMovementRulesSummary =
                    await
                        mediator.SendAsync(new PerformBulkUploadContentValidation(bulkMovementRulesSummary,
                            notificationId, DataTable, file.FileName, isCsv));
            }
            return bulkMovementRulesSummary;
        }

        public async Task<BulkMovementRulesSummary> GetShipmentMovementValidationSummary(HttpPostedFileBase file, Guid notificationId)
        {
            var resultFileRules = await GetFileRules(file, FileUploadType.ShipmentMovementDocuments);

            var extension = Path.GetExtension(file.FileName);
            var isCsv = extension == ".csv" ? true : false;

            var bulkMovementRulesSummary = new BulkMovementRulesSummary(resultFileRules);
            
            return bulkMovementRulesSummary;
        }

        private async Task<List<RuleResult<BulkMovementFileRules>>> GetFileRules(HttpPostedFileBase file, FileUploadType type)
        {
            var rules = new List<RuleResult<BulkMovementFileRules>>();

            foreach (var rule in fileRules.Where(p => p.UploadType.Contains(type)))
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