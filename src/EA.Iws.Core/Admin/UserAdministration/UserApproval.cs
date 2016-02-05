namespace EA.Iws.Core.Admin.UserAdministration
{
    using System;
    using Authorization;

    public class UserApproval
    {
        public UserApproval(Guid userId, ApprovalAction approvalAction, UserRole? assignedRole)
        {
            UserId = userId;
            ApprovalAction = approvalAction;
            AssignedRole = assignedRole;
        }

        public Guid UserId { get; private set; }

        public ApprovalAction ApprovalAction { get; private set; }

        public UserRole? AssignedRole { get; private set; }
    }
}