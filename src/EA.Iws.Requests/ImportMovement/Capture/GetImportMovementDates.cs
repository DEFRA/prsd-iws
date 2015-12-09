namespace EA.Iws.Requests.ImportMovement.Capture
{
    using System;
    using Core.ImportMovement;
    using Prsd.Core.Mediator;
    
    public class GetImportMovementDates : IRequest<ImportMovementData>
    {
        public Guid MovementId { get; private set; }

        public GetImportMovementDates(Guid movementId)
        {
            MovementId = movementId;
        }
    }
}
