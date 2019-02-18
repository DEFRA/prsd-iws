namespace EA.Iws.Web.Infrastructure.BulkReceiptRecovery
{
    using System;
    using System.Threading.Tasks;
    using System.Web;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Movement.BulkUpload;

    public interface IReceiptRecoveryValidator
    {
        Task<ReceiptRecoveryRulesSummary> GetValidationSummary(HttpPostedFileBase file, Guid notificationId);
        Task<BulkFileRulesSummary> GetShipmentMovementValidationSummary(HttpPostedFileBase file, Guid notificationId);
    }
}