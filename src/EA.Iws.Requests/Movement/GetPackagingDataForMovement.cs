namespace EA.Iws.Requests.Movement
{
    using System;
    using EA.Iws.Core.PackagingType;
    using EA.Prsd.Core.Mediator;

    public class GetPackagingDataForMovement : IRequest<PackagingData>
    {
        public Guid MovementId { get; private set; }

        public GetPackagingDataForMovement(Guid movementId)
        {
            MovementId = movementId;
        }
    }
}
