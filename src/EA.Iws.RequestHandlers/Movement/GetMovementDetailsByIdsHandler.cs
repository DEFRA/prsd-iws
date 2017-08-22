namespace EA.Iws.RequestHandlers.Movement
{
    using Core.Movement;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    internal class GetMovementDetailsByIdsHandler : IRequestHandler<GetMovementDetailsByIds, MovementBasicDetails>
    {
        private readonly IMovementDetailsRepository movementDetailsRepository;
        private readonly IMovementRepository movementRepository;

        public GetMovementDetailsByIdsHandler(IMovementRepository movementRepository,
          IMovementDetailsRepository movementDetailsRepository)
        {
            this.movementRepository = movementRepository;
            this.movementDetailsRepository = movementDetailsRepository;
        }

        public async Task<MovementBasicDetails> HandleAsync(GetMovementDetailsByIds message)
        {
            var result = new MovementBasicDetails();

            var movement = await movementRepository.GetById(message.MovementIds);
          
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
