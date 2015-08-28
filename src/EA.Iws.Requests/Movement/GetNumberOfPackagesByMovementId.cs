namespace EA.Iws.Requests.Movement
{
    using System;
    using Prsd.Core.Mediator;

    public class GetNumberOfPackagesByMovementId : IRequest<int?>
    {
        public Guid Id { get; private set; }

        public GetNumberOfPackagesByMovementId(Guid id)
        {
            Id = id;
        }
    }
}
