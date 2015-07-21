namespace EA.Iws.Requests.Admin.UserAdministration
{
    using System.Collections.Generic;
    using Core.Admin;
    using Prsd.Core.Mediator;

    public class SetUserApprovals : IRequest<bool>
    {
        public IList<KeyValuePair<string, ApprovalAction>> UserActions { get; private set; }

        public SetUserApprovals(IList<KeyValuePair<string, ApprovalAction>> userActions)
        {
            this.UserActions = userActions;
        }
    }
}
