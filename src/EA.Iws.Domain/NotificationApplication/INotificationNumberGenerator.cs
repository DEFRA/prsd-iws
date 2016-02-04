namespace EA.Iws.Domain.NotificationApplication
{
    using System.Threading.Tasks;
    using CompetentAuthorityEnum = Core.Notification.UKCompetentAuthority;

    public interface INotificationNumberGenerator
    {
        Task<int> GetNextNotificationNumber(CompetentAuthorityEnum competentAuthority);
    }
}