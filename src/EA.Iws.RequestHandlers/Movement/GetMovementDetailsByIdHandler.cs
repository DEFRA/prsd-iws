namespace EA.Iws.RequestHandlers.Movement
{
    using Core.Movement;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using System;
    using System.Threading.Tasks;

    internal class GetMovementDetailsByIdHandler : IRequestHandler<GetMovementDetailsById, MovementBasicDetails>
    {
        private readonly IMovementRepository movementRepository;

        public GetMovementDetailsByIdHandler(IMovementRepository movementRepository)
        {
            this.movementRepository = movementRepository;
        }

        public async Task<MovementBasicDetails> HandleAsync(GetMovementDetailsById message)
        {
            var result = new MovementBasicDetails();

            var movement = await movementRepository.GetById(message.MovementId);
          
            result = new MovementBasicDetails
            {
                Id = movement.Id,
                Number = movement.Number,
                ActualDate = movement.Date,
                ReceiptDate = movement.Receipt != null ? movement.Receipt.Date : (DateTime?)null
            };
            return result;
        }
    }
}
