namespace EA.Iws.RequestHandlers.Movement
{
    using System.Threading.Tasks;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class GetMovementNumberByMovementIdHandler : IRequestHandler<GetMovementNumberByMovementId, int>
    {
        private readonly IMovementRepository movementRepository;

        public GetMovementNumberByMovementIdHandler(IMovementRepository movementRepository)
        {
            this.movementRepository = movementRepository;
        }

        public async Task<int> HandleAsync(GetMovementNumberByMovementId message)
        {
            var movement = await movementRepository.GetById(message.MovementId);

            return movement.Number;
        }
    }
}
