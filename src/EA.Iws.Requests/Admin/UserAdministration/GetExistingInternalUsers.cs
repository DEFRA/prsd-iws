namespace EA.Iws.Requests.Admin.UserAdministration
{
    using Core.Admin;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(UserAdministrationPermissions.CanManageExistingInternalUser)]
    public class GetExistingInternalUsers : IRequest<InternalUserData[]>
    {
    }
}