namespace EA.Iws.Web.Infrastructure.BulkUpload
{
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using Core.Movement.Bulk;
    using Core.Rules;

    public class PrenotificationFileTypeRule : IBulkMovementPrenotificationFileRule
    {
        private readonly string[] allowedTypes;

        public DataTable DataTable { get; set; }

        public PrenotificationFileTypeRule()
        {
            allowedTypes = new[] 
            {
                MimeTypes.MSExcel,
                MimeTypes.MSExcelXml,
                MimeTypes.Csv
            };
        }

        public async Task<RuleResult<BulkMovementFileRules>> GetResult(HttpPostedFileBase file)
        {
            return await Task.Run(() =>
            {
                var result = allowedTypes.Contains(file.ContentType) ? MessageLevel.Success : MessageLevel.Error;

                return new RuleResult<BulkMovementFileRules>(BulkMovementFileRules.FileType, result);
            });
        }
    }
}