namespace EA.Iws.RequestHandlers.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using DataAccess;
    using Domain.Movement;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class CancelMovementsHandler : IRequestHandler<CancelMovements, bool>
    {
        private readonly IwsContext context;
        private readonly IMovementRepository repository;
        private readonly IMovementAuditRepository movementAuditRepository;
        private readonly IUserContext userContext;
        private readonly ICapturedMovementFactory capturedMovementFactory;

        public CancelMovementsHandler(IwsContext context, IMovementRepository repository,
            IMovementAuditRepository movementAuditRepository,
            IUserContext userContext,
            ICapturedMovementFactory capturedMovementFactory)
        {
            this.repository = repository;
            this.context = context;
            this.movementAuditRepository = movementAuditRepository;
            this.userContext = userContext;
            this.capturedMovementFactory = capturedMovementFactory;
        }

        public async Task<bool> HandleAsync(CancelMovements message)
        {
            var movementIds = message.CancelledMovements.Select(m => m.Id).ToList();

            movementIds.AddRange((await CaptureAddedMovements(message)).Select(m => m.Id));

            var movements = (await repository.GetMovementsByIds(message.NotificationId, movementIds)).ToList();

            foreach (var movement in movements)
            {
                movement.Cancel();
            }

            await context.SaveChangesAsync();

            foreach (var movement in movements)
            {
                await movementAuditRepository.Add(new MovementAudit(movement.NotificationId, movement.Number,
                    userContext.UserId.ToString(), (int)MovementAuditType.Cancelled, SystemTime.Now));
            }

            await context.SaveChangesAsync();

            return true;
        }

        private async Task<List<Movement>> CaptureAddedMovements(CancelMovements message)
        {
            var result = new List<Movement>();

            foreach (var addedMovement in message.AddedMovements)
            {
                var movement = await capturedMovementFactory.Create(message.NotificationId, addedMovement.Number,
                    null, addedMovement.ShipmentDate, true);

                movement.HasNoPrenotification = false;
                movement.SubmitInternally(SystemTime.Now.Date);

                repository.Add(movement);

                await context.SaveChangesAsync();

                result.Add(movement);
            }

            foreach (var movement in result)
            {
                await movementAuditRepository.Add(new MovementAudit(movement.NotificationId, movement.Number,
                    userContext.UserId.ToString(), (int)MovementAuditType.Prenotified, SystemTime.Now));
            }

            await context.SaveChangesAsync();

            return result;
        }
    }
}
