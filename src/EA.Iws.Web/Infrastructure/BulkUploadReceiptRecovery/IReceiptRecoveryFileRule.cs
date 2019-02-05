namespace EA.Iws.Web.Infrastructure.BulkUploadReceiptRecovery
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using System.Web;
    using Core.Documents;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Rules;

    public interface IReceiptRecoveryFileRule
    {
        List<FileUploadType> UploadType { get; }
        DataTable DataTable { get; set; }
        Task<RuleResult<ReceiptRecoveryFileRules>> GetResult(HttpPostedFileBase file);
    }
}