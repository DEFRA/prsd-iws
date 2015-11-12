namespace EA.Iws.RequestHandlers.Movement
{
    using System;
    using System.Threading.Tasks;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class GetMovementDateByMovementIdHandler : IRequestHandler<GetMovementDateByMovementId, DateTime>
    {
        private readonly IMovementRepository repository;

        public GetMovementDateByMovementIdHandler(IMovementRepository repository)
        {
            this.repository = repository;
        }

        public async Task<DateTime> HandleAsync(GetMovementDateByMovementId message)
        {
            var movement = await repository.GetById(message.MovementId);

            return movement.Date;
        }
    }
}