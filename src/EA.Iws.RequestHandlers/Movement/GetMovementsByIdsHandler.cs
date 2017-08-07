namespace EA.Iws.RequestHandlers.Movement
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class GetMovementsByIdsHandler : IRequestHandler<GetMovementsByIds, MovementInfo[]>
    {
        private readonly IMovementDetailsRepository movementDetailsRepository;
        private readonly IMovementRepository movementRepository;

        public GetMovementsByIdsHandler(IMovementRepository movementRepository,
            IMovementDetailsRepository movementDetailsRepository)
        {
            this.movementRepository = movementRepository;
            this.movementDetailsRepository = movementDetailsRepository;
        }

        public async Task<MovementInfo[]> HandleAsync(GetMovementsByIds message)
        {
            var result = new List<MovementInfo>();

            var movements = await movementRepository.GetMovementsByIds(message.NotificationId, message.MovementIds);

            foreach (var movement in movements)
            {
                var details = await movementDetailsRepository.GetByMovementId(movement.Id);

                result.Add(new MovementInfo
                {
                    Id = movement.Id,
                    ActualQuantity = details.ActualQuantity.Quantity,
                    Unit = details.ActualQuantity.Units,
                    PackagingTypes = details.PackagingInfos.Select(p => p.PackagingType),
                    ShipmentNumber = movement.Number
                });
            }

            return result.ToArray();
        }
    }
}