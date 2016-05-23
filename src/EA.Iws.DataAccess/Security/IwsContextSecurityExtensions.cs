namespace EA.Iws.DataAccess.Security
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Notification;
    using Prsd.Core.Domain;

    internal static class IwsContextSecurityExtensions
    {
        public static async Task<UKCompetentAuthority> GetUsersCompetentAuthority(this IwsContext context,
            IUserContext userContext)
        {
            return await context.InternalUsers
                .Where(u => u.UserId == userContext.UserId.ToString())
                .Select(u => u.CompetentAuthority)
                .SingleAsync();
        }
    }
}