namespace EA.Iws.Requests.MovementReceipt
{
    using System;
    using Core.MovementReceipt;
    using Prsd.Core.Mediator;

    public class UpdateShipmentAcceptanceDataByMovementId : IRequest<bool>
    {
        public Guid MovementId { get; private set; }
        public Decision Decision { get; private set; }
        public string RejectReason { get; private set; }

        public UpdateShipmentAcceptanceDataByMovementId(Guid movementId, Decision decision, string rejectReason)
        {
            MovementId = movementId;
            Decision = decision;
            RejectReason = rejectReason;
        }
    }
}
