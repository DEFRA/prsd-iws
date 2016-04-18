namespace EA.Iws.RequestHandlers
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Domain;

    public static class UserExtensions
    {
        public static async Task<bool> IsInternalUserAsync(this IwsContext context, 
            IUserContext userContext)
        {
            if (userContext == null)
            {
                return false;
            }

            return await context.InternalUsers.AnyAsync(u => 
                u.UserId == userContext.UserId.ToString());
        }
    }
}
