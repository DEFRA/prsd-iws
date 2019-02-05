namespace EA.Iws.Web.Infrastructure.BulkUploadReceiptRecovery
{
    using System;
    using System.Data;
    using System.Threading.Tasks;
    using System.Web;
    using Core.Movement.BulkReceiptRecovery;

    public interface IReceiptRecoveryValidator
    {
        DataTable DataTable { get; set; }
        Task<ReceiptRecoveryRulesSummary> GetValidationSummary(HttpPostedFileBase file, Guid notificationId);
        Task<ReceiptRecoveryRulesSummary> GetShipmentMovementValidationSummary(HttpPostedFileBase file, Guid notificationId);
    }
}