namespace EA.Iws.DataAccess.Repositories
{
    using Domain.NotificationApplication;

    public class UserHistoryRepository : IUserHistoryRepository
    {
        private readonly IwsContext context;

        public UserHistoryRepository(IwsContext context)
        {
            this.context = context;
        }

        public void AddUserChangeData(UserHistory userHistory)
        {
            context.UserHistory.Add(userHistory);
        }
    }
}
