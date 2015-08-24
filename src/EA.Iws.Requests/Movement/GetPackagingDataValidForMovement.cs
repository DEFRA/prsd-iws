namespace EA.Iws.Requests.Movement
{
    using System;
    using EA.Iws.Core.PackagingType;
    using EA.Prsd.Core.Mediator;

    public class GetPackagingDataValidForMovement : IRequest<PackagingData>
    {
        public Guid MovementId { get; private set; }

        public GetPackagingDataValidForMovement(Guid movementId)
        {
            MovementId = movementId;
        }
    }
}
