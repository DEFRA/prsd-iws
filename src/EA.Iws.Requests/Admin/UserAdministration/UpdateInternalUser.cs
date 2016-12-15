namespace EA.Iws.Requests.Admin.UserAdministration
{
    using Core.Admin;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(UserAdministrationPermissions.CanManageExistingInternalUser)]
    public class UpdateInternalUser : IRequest<Unit>
    {
        public UpdateInternalUser(string userId, InternalUserStatus status, UserRole role)
        {
            UserId = userId;
            Status = status;
            Role = role;
        }

        public string UserId { get; private set; }

        public InternalUserStatus Status { get; private set; }

        public UserRole Role { get; private set; }
    }
}