namespace EA.Iws.Web.Infrastructure.BulkReceiptRecovery
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using System.Web;
    using Core.Documents;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Rules;

    public class ReceiptRecoveryFileSizeRule : IReceiptRecoveryFileRule
    {   
        public DataTable DataTable { get; set; }

        public List<FileUploadType> UploadType
        {
            get
            {
                var x = new List<FileUploadType>()
                {
                    FileUploadType.ReceiptRecovery,
                    FileUploadType.ShipmentMovementDocuments
                };

                return x;
            }
        }

        public async Task<RuleResult<ReceiptRecoveryFileRules>> GetResult(HttpPostedFileBase file)
        {
            return await Task.Run(() =>
            {
                var result = file.ContentLength < int.MaxValue ? MessageLevel.Success : MessageLevel.Error;
                return new RuleResult<ReceiptRecoveryFileRules>(ReceiptRecoveryFileRules.FileSize, result);
            });
        }
    }
}