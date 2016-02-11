namespace EA.Iws.Requests.Admin.UserAdministration
{
    using System.Collections.Generic;
    using Core.Admin.UserAdministration;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(UserAdministrationPermissions.CanApproveNewInternalUser)]
    public class SetUserApprovals : IRequest<bool>
    {
        public SetUserApprovals(IList<UserApproval> userApprovals)
        {
            UserApprovals = userApprovals;
        }

        public IList<UserApproval> UserApprovals { get; private set; }
    }
}