namespace EA.Iws.Requests.Movement.Reject
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanEditExportMovementsInternal)]
    public class RecordRejectionInternal : IRequest<bool>
    {
        public Guid MovementId { get; private set; }

        public string RejectionReason { get; private set; }

        public DateTime RejectedDate { get; private set; }

        public RecordRejectionInternal(Guid movementId, 
            DateTime rejectedDate, 
            string rejectionReason)
        {
            Guard.ArgumentNotNullOrEmpty(() => rejectionReason, rejectionReason);

            MovementId = movementId;
            RejectedDate = rejectedDate;
            RejectionReason = rejectionReason;
        }
    }
}