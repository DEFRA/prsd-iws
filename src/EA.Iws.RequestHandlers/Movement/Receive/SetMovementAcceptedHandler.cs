namespace EA.Iws.RequestHandlers.Movement.Receive
{
    using System;
    using System.Threading.Tasks;
    using Core.Movement;
    using DataAccess;
    using Domain.Movement;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.Movement.Receive;

    internal class SetMovementAcceptedHandler : IRequestHandler<SetMovementAccepted, Guid>
    {
        private readonly IMovementRepository movementRepository;
        private readonly IwsContext context;
        private readonly IUserContext userContext;
        private readonly IMovementAuditRepository movementAuditRepository;

        public SetMovementAcceptedHandler(IMovementRepository movementRepository, IwsContext context,
            IUserContext userContext, IMovementAuditRepository movementAuditRepository)
        {
            this.movementRepository = movementRepository;
            this.context = context;
            this.userContext = userContext;
            this.movementAuditRepository = movementAuditRepository;
        }

        public async Task<Guid> HandleAsync(SetMovementAccepted message)
        {
            var movement = await movementRepository.GetById(message.MovementId);

            movement.Receive(message.FileId, message.DateReceived, new Domain.ShipmentQuantity(message.Quantity, message.Units), userContext.UserId);

            await context.SaveChangesAsync();

            await
                movementAuditRepository.Add(new MovementAudit(movement.NotificationId, movement.Number,
                    userContext.UserId.ToString(), (int)MovementAuditType.Received, SystemTime.Now));

            await context.SaveChangesAsync();

            return message.MovementId;
        }
    }
}
