namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.Movement;

    internal class MovementRejectionRepository : IMovementRejectionRepository
    {
        private readonly IwsContext context;

        public MovementRejectionRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<MovementRejection> GetByMovementId(Guid movementId)
        {
            return await context.MovementRejections.SingleAsync(r => r.MovementId == movementId);
        }

        public async Task<MovementRejection> Get(Guid id)
        {
            return await context.MovementRejections.SingleAsync(r => r.Id == id);
        }

        public async Task<MovementRejection> GetByMovementIdOrDefault(Guid movementId)
        {
            return await context.MovementRejections.SingleOrDefaultAsync(m => m.MovementId == movementId);
        }

        public void Add(MovementRejection movementRejection)
        {
            context.MovementRejections.Add(movementRejection);
        }
    }
}
