namespace EA.Iws.Requests.NotificationMovements.Create
{
    using System;
    using Core.Movement;
    using Prsd.Core.Mediator;

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