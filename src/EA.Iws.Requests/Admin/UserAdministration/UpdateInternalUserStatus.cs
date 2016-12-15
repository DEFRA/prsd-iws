namespace EA.Iws.Requests.Admin.UserAdministration
{
    using Core.Admin;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(UserAdministrationPermissions.CanManageExistingInternalUser)]
    public class UpdateInternalUserStatus : IRequest<Unit>
    {
        public UpdateInternalUserStatus(string userId, InternalUserStatus status)
        {
            UserId = userId;
            Status = status;
        }

        public string UserId { get; private set; }

        public InternalUserStatus Status { get; private set; }
    }
}