namespace EA.Iws.DataAccess.Repositories
{
    using Domain.NotificationApplication;

    public class SharedUserRepository : ISharedUserRepository
    {
        private readonly IwsContext context;

        public SharedUserRepository(IwsContext context)
        {
            this.context = context;
        }

        public void AddSharedUser(SharedUser sharedUser)
        {
            this.context.SharedUser.Add(sharedUser);
        }

        public void RemoveSharedUser(SharedUser sharedUser)
        {
            this.context.SharedUser.Remove(sharedUser);
        }
    }
}
