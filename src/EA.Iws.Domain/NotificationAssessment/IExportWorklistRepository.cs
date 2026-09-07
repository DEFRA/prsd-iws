namespace EA.Iws.Domain.NotificationAssessment
{
    using Core.Notification;
    using Core.NotificationAssessment;
    using System.Threading.Tasks;

    public interface IExportWorklistRepository
    {
        Task<ExportWorklistQueryResult> GetByCompetentAuthority(
            UKCompetentAuthority competentAuthority,
            string notificationNumber,
            string officer,
            NotificationStatus[] statuses,
            int pageNumber,
            int pageSize);
    }
}