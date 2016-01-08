namespace EA.Iws.Requests.Movement.Complete
{
    using System;
    using Core.Movement;
    using Prsd.Core.Mediator;

    public class GetOperationCompleteData : IRequest<OperationCompleteData>
    {
        public Guid MovementId { get; private set; }

        public GetOperationCompleteData(Guid movementId)
        {
            MovementId = movementId;
        }
    }
}
