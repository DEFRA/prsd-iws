namespace EA.Iws.Web.Infrastructure.BulkUpload
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using System.Web;
    using Core.Movement.Bulk;
    using Core.Rules;
    using VirusScanning;

    public class BulkMovementValidator : IBulkMovementValidator
    {
        private readonly IFileReader fileReader;

        public BulkMovementValidator(IFileReader fileReader)
        {
            this.fileReader = fileReader;
        }

        public async Task<BulkMovementRulesSummary> GetValidationSummary(HttpPostedFileBase file)
        {
            var fileRules = await GetFileRules(file);

            return new BulkMovementRulesSummary(fileRules);
        }

        private async Task<List<RuleResult<BulkMovementFileRules>>> GetFileRules(HttpPostedFileBase file)
        {
            var rules = new List<RuleResult<BulkMovementFileRules>>();
            var fileExtension = Path.GetExtension(file.FileName);

            var fileTypeResult = MessageLevel.Success;
            if (string.IsNullOrEmpty(fileExtension) || fileExtension != ".xlsx" || fileExtension != ".csv")
            {
                fileTypeResult = MessageLevel.Error;
            }

            var fileSizeResult = MessageLevel.Success;
            var fileVirusScanResult = MessageLevel.Success;

            try
            {
                // TODO: Check for file size.
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