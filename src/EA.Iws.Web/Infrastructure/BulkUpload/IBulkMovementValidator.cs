namespace EA.Iws.Web.Infrastructure.BulkUpload
{
    using System;
    using System.Data;
    using System.Threading.Tasks;
    using System.Web;
    using Core.Movement.Bulk;

    public interface IBulkMovementValidator
    {
        DataTable DataTable { get; set; }
        Task<BulkMovementRulesSummary> GetValidationSummary(HttpPostedFileBase file, Guid notificationId);
    }
}