namespace EA.Iws.Requests.Admin.UserAdministration
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(UserAdministrationPermissions.CanManageExistingInternalUser)]
    public class UpdateInternalUserRole : IRequest<Unit>
    {
        public UpdateInternalUserRole(string userId, UserRole role)
        {
            UserId = userId;
            Role = role;
        }

        public string UserId { get; private set; }

        public UserRole Role { get; private set; }
    }
}