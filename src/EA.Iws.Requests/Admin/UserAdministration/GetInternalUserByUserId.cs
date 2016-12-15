namespace EA.Iws.Requests.Admin.UserAdministration
{
    using Core.Admin;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(UserAdministrationPermissions.CanManageExistingInternalUser)]
    public class GetInternalUserByUserId : IRequest<InternalUserData>
    {
        public GetInternalUserByUserId(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; private set; }
    }
}