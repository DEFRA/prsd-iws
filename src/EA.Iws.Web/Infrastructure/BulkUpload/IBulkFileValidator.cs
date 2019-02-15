namespace EA.Iws.Web.Infrastructure.BulkUpload
{
    using System.Threading.Tasks;
    using System.Web;
    using Core.Movement.BulkUpload;

    public interface IBulkFileValidator
    {
        Task<BulkFileRulesSummary> GetFileRulesSummary(HttpPostedFileBase file, BulkFileType type);
    }
}