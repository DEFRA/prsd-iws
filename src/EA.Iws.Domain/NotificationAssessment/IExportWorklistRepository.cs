namespace EA.Iws.Domain.NotificationAssessment
{
    using System.Threading.Tasks;
    using Core.Notification;
    using Core.NotificationAssessment;

    public interface IExportWorklistRepository
    {
        Task<ExportWorklistQueryResult> GetByCompetentAuthority(
            UKCompetentAuthority competentAuthority,
            string notificationNumber,
            string officer,
            NotificationStatus? status,
            int pageNumber,
            int pageSize);
    }

    public class ExportWorklistQueryResult
    {
        public ExportWorklistSummary[] PagedRows { get; set; }
        public int TotalCount { get; set; }
    }
}