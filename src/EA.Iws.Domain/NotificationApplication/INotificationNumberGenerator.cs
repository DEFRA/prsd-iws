namespace EA.Iws.Domain.NotificationApplication
{
    using System.Threading.Tasks;

    public interface INotificationNumberGenerator
    {
        Task<int> GetNextNotificationNumber(UKCompetentAuthority competentAuthority);
    }
}