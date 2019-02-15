namespace EA.Iws.Web.Infrastructure.BulkReceiptRecovery
{
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using Core.Documents;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Rules;

    public class ReceiptRecoveryFileTypeRule : IReceiptRecoveryFileRule
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

        public ReceiptRecoveryFileTypeRule()
        {
            allowedTypes = new[] 
            {
                ".xlsx",
                ".xls",
                ".csv"
            };
        }

        public async Task<RuleResult<ReceiptRecoveryFileRules>> GetResult(HttpPostedFileBase file)
        {
            return await Task.Run(() =>
            {
                var extension = Path.GetExtension(file.FileName);

                var result = allowedTypes.Contains(extension) ? MessageLevel.Success : MessageLevel.Error;

                return new RuleResult<ReceiptRecoveryFileRules>(ReceiptRecoveryFileRules.FileTypeReceiptRecovery, result);
            });
        }
    }
}