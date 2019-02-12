namespace EA.Iws.Web.Infrastructure.BulkPrenotification
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using Core.Documents;
    using Core.Movement.BulkPrenotification;
    using Core.Rules;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.BulkUpload;
    using VirusScanning;

    public class PrenotificationValidator : IPrenotificationValidator
    {
        private readonly IMediator mediator;
        private readonly IEnumerable<IPrenotificationFileRule> fileRules;
        private readonly IVirusScanner virusScanner;

        public DataTable DataTable { get; set; }

        public byte[] FileBytes { get; set; }

        public PrenotificationValidator(IMediator mediator, 
            IEnumerable<IPrenotificationFileRule> fileRules,
            IVirusScanner virusScanner)
        {
            this.mediator = mediator;
            this.fileRules = fileRules;
            this.virusScanner = virusScanner;
        }

        public async Task<PrenotificationRulesSummary> GetPrenotificationValidationSummary(HttpPostedFileBase file, Guid notificationId)
        {
            var resultFileRules = await GetFileRules(file, FileUploadType.Prenotification);

            var extension = Path.GetExtension(file.FileName);
            var isCsv = extension == ".csv";

            var rulesSummary = new PrenotificationRulesSummary(resultFileRules);

            if (rulesSummary.IsFileRulesSuccess)
            {
                rulesSummary =
                    await
                        mediator.SendAsync(new PerformPrenotificationContentValidation(rulesSummary,
                            notificationId, DataTable, file.FileName, isCsv));
            }
            return rulesSummary;
        }

        public async Task<PrenotificationRulesSummary> GetShipmentMovementValidationSummary(HttpPostedFileBase file, Guid notificationId)
        {
            var resultFileRules = await GetFileRules(file, FileUploadType.ShipmentMovementDocuments);

            var bulkMovementRulesSummary = new PrenotificationRulesSummary(resultFileRules) { FileBytes = FileBytes };

            return bulkMovementRulesSummary;
        }

        private async Task<List<RuleResult<PrenotificationFileRules>>> GetFileRules(HttpPostedFileBase file, FileUploadType type)
        {
            var rules = new List<RuleResult<PrenotificationFileRules>>();

            foreach (var rule in fileRules.Where(p => p.UploadType.Contains(type)))
            {
                var result = await rule.GetResult(file);
                // Grab the DataTable from the File Parse rule.
                if (result.Rule == PrenotificationFileRules.FileParse)
                {
                    DataTable = rule.DataTable;
                }

                rules.Add(result);
            }

            rules.Add(await GetVirusScanResult(file));

            if (DataTable != null && DataTable.Rows.Count == 0)
            {
                rules.Add(new RuleResult<PrenotificationFileRules>(PrenotificationFileRules.EmptyData, MessageLevel.Error));
            }
            
            return rules;
        }

        private async Task<RuleResult<PrenotificationFileRules>> GetVirusScanResult(HttpPostedFileBase file)
        {
            var result = MessageLevel.Success;
            byte[] fileBytes;

            using (var memoryStream = new MemoryStream())
            {
                await file.InputStream.CopyToAsync(memoryStream);

                fileBytes = memoryStream.ToArray();

                FileBytes = fileBytes;
            }

            if (await virusScanner.ScanFileAsync(fileBytes) == ScanResult.Virus)
            {
                result = MessageLevel.Error;
            }

            return new RuleResult<PrenotificationFileRules>(PrenotificationFileRules.Virus, result);
        }
    }
}