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
            var contentRules = new List<ContentRuleResult<BulkMovementContentRules>>();
            var fileRules = await GetFileRules(file);
            // COULLM: Suspect we need a way identify, and thus prevent GetContentRules from being run if we have at least one error in the FileRules
            contentRules = GetContentRules();

            return new BulkMovementRulesSummary(fileRules, contentRules);
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

        private List<ContentRuleResult<BulkMovementContentRules>> GetContentRules()
        {
            var rules = new List<ContentRuleResult<BulkMovementContentRules>>();

            rules.Add(new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.MissingData, MessageLevel.Error, new List<string> { "1", "2" }));

            return rules;
        }
    }
}