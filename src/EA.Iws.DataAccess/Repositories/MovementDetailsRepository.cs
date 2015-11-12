namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Threading.Tasks;
    using Domain.Movement;

    internal class MovementDetailsRepository : IMovementDetailsRepository
    {
        public Task<MovementDetails> GetByMovementId(Guid movementId)
        {
            throw new NotImplementedException();
        }
    }
}