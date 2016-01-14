namespace EA.Iws.Domain.NotificationApplication
{
    public interface IUserHistoryRepository
    {
        void AddUserChangeData(UserHistory userHistory);
    }
}
