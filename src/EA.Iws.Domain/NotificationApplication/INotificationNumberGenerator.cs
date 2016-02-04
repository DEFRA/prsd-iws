namespace EA.Iws.Domain.NotificationApplication
{
    using System.Threading.Tasks;
    using Core.Notification;

    public interface INotificationNumberGenerator
    {
        Task<int> GetNextNotificationNumber(UKCompetentAuthority competentAuthority);
    }
}