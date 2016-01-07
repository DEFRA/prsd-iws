namespace EA.Iws.Requests.Movement.Edit
{
    using System;
    using Core.Movement;
    using Prsd.Core.Mediator;

    public class IsProposedUpdatedMovementDateValid : IRequest<ProposedUpdatedMovementDateResponse>
    {
        public Guid MovementId { get; private set; }

        public DateTime ProposedDate { get; private set; }

        public IsProposedUpdatedMovementDateValid(Guid movementId, DateTime proposedDate)
        {
            ProposedDate = proposedDate;
            MovementId = movementId;
        }
    }
}