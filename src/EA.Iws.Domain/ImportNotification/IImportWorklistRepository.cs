namespace EA.Iws.Domain.ImportNotificationAssessment
{
    using Core.ImportNotificationAssessment;
    using Core.Notification;
    using System.Threading.Tasks;

    public interface IImportWorklistRepository
    {
        Task<ImportWorklistQueryResult> GetByCompetentAuthority(
            UKCompetentAuthority competentAuthority,
            string notificationNumber,
            string officer,
            ImportNotificationStatus[] statuses,
            int pageNumber,
            int pageSize);
    }
}