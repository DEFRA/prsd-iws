namespace EA.Iws.RequestHandlers.Authorization
{
    using System.Security.Claims;

    public interface IUserRoleService
    {
        UserRole Get(ClaimsPrincipal claimsPrincipal);

        UserRole Get(string userid);
    }
}