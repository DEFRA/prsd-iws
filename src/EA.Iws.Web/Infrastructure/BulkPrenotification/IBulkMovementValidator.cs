namespace EA.Iws.Web.Infrastructure.BulkPrenotification
{
    using System;
    using System.Data;
    using System.Threading.Tasks;
    using System.Web;
    using Core.Movement.BulkPrenotification;

    public interface IBulkMovementValidator
    {
        DataTable DataTable { get; set; }
        Task<BulkMovementRulesSummary> GetPrenotificationValidationSummary(HttpPostedFileBase file, Guid notificationId);
        Task<BulkMovementRulesSummary> GetShipmentMovementValidationSummary(HttpPostedFileBase file, Guid notificationId);
    }
}