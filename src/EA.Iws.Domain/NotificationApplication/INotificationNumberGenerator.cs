namespace EA.Iws.Domain.Notification
{
    using System.Threading.Tasks;

    public interface INotificationNumberGenerator
    {
        Task<int> GetNextNotificationNumber(UKCompetentAuthority competentAuthority);
    }
}