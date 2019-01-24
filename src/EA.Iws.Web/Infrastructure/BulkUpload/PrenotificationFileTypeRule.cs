namespace EA.Iws.Web.Infrastructure.BulkUpload
{
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Threading.Tasks;
    using System.Web;
    using Core.Movement.Bulk;
    using Core.Rules;

    public class PrenotificationFileTypeRule : IBulkMovementPrenotificationFileRule
    {
        public DataTable DataTable { get; set; }

        public async Task<RuleResult<BulkMovementFileRules>> GetResult(HttpPostedFileBase file)
        {
            return await Task.Run(() =>
            {
                var fileExtension = Path.GetExtension(file.FileName).ToLower();
                var validExtensions = new List<string>
                {
                    "xls",
                    ".xslx",
                    ".csv"
                };

                var result = MessageLevel.Success;
                if (string.IsNullOrEmpty(fileExtension) || !validExtensions.Contains(fileExtension))
                {
                    result = MessageLevel.Error;
                }

                return new RuleResult<BulkMovementFileRules>(BulkMovementFileRules.FileType, result);
            });
        }
    }
}