namespace EA.Iws.Requests.Movement
{
    using System;
    using Prsd.Core.Mediator;

    public class SetNumberOfPackagesByMovementId : IRequest<bool>
    {
        public Guid Id { get; private set; }
        public int NumberOfPackages { get; private set; }

        public SetNumberOfPackagesByMovementId(Guid id, int numberOfPackages)
        {
            Id = id;
            NumberOfPackages = numberOfPackages;
        }
    }
}
