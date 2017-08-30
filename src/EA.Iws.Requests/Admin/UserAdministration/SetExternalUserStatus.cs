namespace EA.Iws.Requests.Admin.UserAdministration
{
    using Core.Admin;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(UserAdministrationPermissions.CanManageExternalUsers)]
    public class SetExternalUserStatus : IRequest<Unit>
    {
        public SetExternalUserStatus(string userId, ExternalUserStatus status)
        {
            UserId = userId;
            Status = status;
        }

        public string UserId { get; private set; }

        public ExternalUserStatus Status { get; private set; }
    }
}