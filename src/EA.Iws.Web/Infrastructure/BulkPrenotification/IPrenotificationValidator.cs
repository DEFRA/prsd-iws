namespace EA.Iws.Web.Infrastructure.BulkPrenotification
{
    using System;
    using System.Threading.Tasks;
    using System.Web;
    using Core.Movement.BulkPrenotification;
    using Core.Movement.BulkUpload;

    public interface IPrenotificationValidator
    {
        Task<PrenotificationRulesSummary> GetPrenotificationValidationSummary(HttpPostedFileBase file, Guid notificationId);
        Task<BulkFileRulesSummary> GetShipmentMovementValidationSummary(HttpPostedFileBase file, Guid notificationId);
    }
}