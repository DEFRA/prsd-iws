namespace EA.Iws.Requests.Admin.UserAdministration
{
    using System;
    using System.Collections.Generic;
    using Core.Admin;
    using Prsd.Core.Mediator;

    public class SetUserApprovals : IRequest<bool>
    {
        public IList<KeyValuePair<Guid, ApprovalAction>> UserActions { get; private set; }

        public SetUserApprovals(IList<KeyValuePair<Guid, ApprovalAction>> userActions)
        {
            this.UserActions = userActions;
        }
    }
}
