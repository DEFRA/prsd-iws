namespace EA.Iws.Requests.Admin.UserAdministration
{
    using System.Collections.Generic;
    using Core.Admin;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(UserAdministrationPermissions.CanReadInternalUserData)]
    public class GetAllUsers : IRequest<IEnumerable<ChangeUserData>>
    {
    }
}