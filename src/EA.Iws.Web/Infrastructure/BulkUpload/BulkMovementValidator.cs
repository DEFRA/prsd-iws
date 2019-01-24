namespace EA.Iws.Web.Infrastructure.BulkUpload
{
    using System.Collections.Generic;
    using System.IO;
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
        private readonly IFileReader fileReader;

        public BulkMovementValidator(IMediator mediator, IFileReader fileReader)
        {
            this.mediator = mediator;
            this.fileReader = fileReader;
        }

        public async Task<BulkMovementRulesSummary> GetValidationSummary(HttpPostedFileBase file)
        {
            var fileRules = await GetFileRules(file);

            var bulkMovementRulesSummary = new BulkMovementRulesSummary(fileRules);
            // COULLM: Suspect we need a way identify, and thus prevent GetContentRules from being run if we have at least one error in the FileRules
            bulkMovementRulesSummary = await mediator.SendAsync(new PerformBulkUploadContentValidation(bulkMovementRulesSummary));
            
            return bulkMovementRulesSummary;
        }

        private async Task<List<RuleResult<BulkMovementFileRules>>> GetFileRules(HttpPostedFileBase file)
        {
            var rules = new List<RuleResult<BulkMovementFileRules>>();
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            var validExtensions = new List<string>
            {
                "xls",
                ".xslx",
                ".csv"
            };

            var fileTypeResult = MessageLevel.Success;
            if (string.IsNullOrEmpty(fileExtension) || !validExtensions.Contains(fileExtension))
            {
                fileTypeResult = MessageLevel.Error;
            }

            var fileSizeResult = MessageLevel.Success;
            // int.MaxValue is 2147483647 bytes which is 2GB
            if (file.ContentLength >= int.MaxValue)
            {
                fileSizeResult = MessageLevel.Error;
            }

            var fileVirusScanResult = MessageLevel.Success;
            try
            {
                var data = await fileReader.GetFileBytes(file);
            }
            catch (VirusFoundException)
            {
                fileVirusScanResult = MessageLevel.Error;
            }

            rules.Add(new RuleResult<BulkMovementFileRules>(BulkMovementFileRules.FileType, fileTypeResult));
            rules.Add(new RuleResult<BulkMovementFileRules>(BulkMovementFileRules.FileSize, fileSizeResult));
            rules.Add(new RuleResult<BulkMovementFileRules>(BulkMovementFileRules.Virus, fileVirusScanResult));

            return rules;
        }
    }
}