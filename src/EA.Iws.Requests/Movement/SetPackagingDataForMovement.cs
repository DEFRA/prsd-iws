namespace EA.Iws.Requests.Movement
{
    using System;
    using System.Collections.Generic;
    using Core.PackagingType;
    using Prsd.Core.Mediator;

    public class SetPackagingDataForMovement : IRequest<Guid>
    {
        public Guid MovementId { get; private set; }
        public List<PackagingType> PackagingTypes { get; private set; }

        public SetPackagingDataForMovement(Guid movementId, List<PackagingType> packagingTypes)
        {
            MovementId = movementId;
            PackagingTypes = packagingTypes;
        }
    }
}
