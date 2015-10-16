namespace EA.Iws.RequestHandlers.Authorization
{
    using System.Threading.Tasks;

    public class AuthorizationManager : IAuthorizationManager
    {
        private readonly IUserRoleService userRoleService;
        private readonly IAuthorizationService authorizationService;

        public AuthorizationManager(IUserRoleService userRoleService, IAuthorizationService authorizationService)
        {
            this.userRoleService = userRoleService;
            this.authorizationService = authorizationService;
        }

        public async Task<bool> CheckAccessAsync(AuthorizationContext context)
        {
            var userRole = userRoleService.Get(context.Principal);

            return await authorizationService.HasAccess(userRole, context.Name);
        }
    }
}
