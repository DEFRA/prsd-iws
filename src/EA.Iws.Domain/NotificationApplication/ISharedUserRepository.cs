namespace EA.Iws.Domain.NotificationApplication
{
    public interface ISharedUserRepository
    {
        void AddSharedUser(SharedUser sharedUser);
        void RemoveSharedUser(SharedUser sharedUser);
    }
}
