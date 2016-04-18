namespace EA.Iws.Domain
{
    using System;
    using System.Threading.Tasks;

    public interface IInternalUserRepository
    {
        Task<InternalUser> GetByUserId(string userId);

        Task<InternalUser> GetByUserId(Guid userId);
    }
}
