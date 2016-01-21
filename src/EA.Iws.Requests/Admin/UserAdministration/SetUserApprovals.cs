namespace EA.Iws.Requests.Admin.UserAdministration
{
    using System;
    using System.Collections.Generic;
    using Core.Admin;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(UserAdministrationPermissions.CanApproveNewInternalUser)]
    public class SetUserApprovals : IRequest<bool>
    {
        public IList<KeyValuePair<Guid, ApprovalAction>> UserActions { get; private set; }

        public SetUserApprovals(IList<KeyValuePair<Guid, ApprovalAction>> userActions)
        {
            this.UserActions = userActions;
        }
    }
}
