namespace EA.Iws.Web.Infrastructure.BulkReceiptRecovery
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using Core.Documents;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Rules;

    public class PrenotificationFileTypeRule : IReceiptRecoveryFileRule
    {
        private readonly string[] allowedTypes;

        public DataTable DataTable { get; set; }

        public List<FileUploadType> UploadType
        {
            get
            {
                var x = new List<FileUploadType>()
                {
                    FileUploadType.ReceiptRecovery
                };

                return x;
            }
        }

        public PrenotificationFileTypeRule()
        {
            allowedTypes = new[] 
            {
                MimeTypes.MSExcel,
                MimeTypes.MSExcelXml,
                MimeTypes.Csv
            };
        }

        public async Task<RuleResult<ReceiptRecoveryFileRules>> GetResult(HttpPostedFileBase file)
        {
            return await Task.Run(() =>
            {
                var result = allowedTypes.Contains(file.ContentType) ? MessageLevel.Success : MessageLevel.Error;

                return new RuleResult<ReceiptRecoveryFileRules>(ReceiptRecoveryFileRules.FileTypeReceiptRecovery, result);
            });
        }

        public string GetErrorMessage()
        {
            return "error";
        }
    }
}