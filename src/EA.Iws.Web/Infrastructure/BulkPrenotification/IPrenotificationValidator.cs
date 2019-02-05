namespace EA.Iws.Web.Infrastructure.BulkPrenotification
{
    using System;
    using System.Data;
    using System.Threading.Tasks;
    using System.Web;
    using Core.Movement.BulkPrenotification;

    public interface IPrenotificationValidator
    {
        DataTable DataTable { get; set; }
        Task<PrenotificationRulesSummary> GetPrenotificationValidationSummary(HttpPostedFileBase file, Guid notificationId);
        Task<PrenotificationRulesSummary> GetShipmentMovementValidationSummary(HttpPostedFileBase file, Guid notificationId);
    }
}