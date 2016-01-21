namespace EA.Iws.Requests.NotificationMovements.Create
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Movement;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanCreateExportMovements)]
    public class IsProposedMovementDateValid : IRequest<ProposedMovementDateResponse>
    {
        public Guid NotificationId { get; private set; }

        public DateTime ProposedDate { get; private set; }

        public IsProposedMovementDateValid(Guid notificationId, DateTime proposedDate)
        {
            NotificationId = notificationId;
            ProposedDate = proposedDate;
        }
    }
}