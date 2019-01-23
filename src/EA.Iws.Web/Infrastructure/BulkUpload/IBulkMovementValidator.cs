namespace EA.Iws.Web.Infrastructure.BulkUpload
{
    using System.Threading.Tasks;
    using System.Web;
    using Core.Movement.Bulk;

    public interface IBulkMovementValidator
    {
        Task<BulkMovementRulesSummary> GetValidationSummary(HttpPostedFileBase file);
    }
}