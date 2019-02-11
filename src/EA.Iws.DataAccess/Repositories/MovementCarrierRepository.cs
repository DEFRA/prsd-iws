namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.Movement;
    using Domain.Security;

    internal class MovementCarrierRepository : IMovementCarrierRepository
    {
        private readonly IwsContext context;
        private readonly IMovementAuthorization authorization;

        public MovementCarrierRepository(IwsContext context, IMovementAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public void Add(MovementCarrier movementCarrier)
        {
            context.MovementCarrier.Add(movementCarrier);
        }
    }
}
