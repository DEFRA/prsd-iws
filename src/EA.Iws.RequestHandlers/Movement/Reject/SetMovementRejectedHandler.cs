namespace EA.Iws.RequestHandlers.Movement.Reject
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.Movement.Reject;

    internal class SetMovementRejectedHandler : IRequestHandler<SetMovementRejected, Guid>
    {
        private readonly IRejectMovement rejectMovement;
        private readonly IwsContext context;

        public SetMovementRejectedHandler(IRejectMovement rejectMovement, IwsContext context)
        {
            this.rejectMovement = rejectMovement;
            this.context = context;
        }

        public async Task<Guid> HandleAsync(SetMovementRejected message)
        {
            var movementRejection = await rejectMovement.Reject(message.MovementId,
                new DateTimeOffset(message.DateReceived),
                message.Reason);

            movementRejection.SetFile(message.FileId);
            
            await context.SaveChangesAsync();

            return message.MovementId;
        }
    }
}
