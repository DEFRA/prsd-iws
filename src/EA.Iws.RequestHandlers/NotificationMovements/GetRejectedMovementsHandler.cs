namespace EA.Iws.RequestHandlers.NotificationMovements
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements;

    internal class GetRejectedMovementsHandler : IRequestHandler<GetRejectedMovements, IList<RejectedMovementListData>>
    {
        private readonly IMovementRepository repository;

        public GetRejectedMovementsHandler(IMovementRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IList<RejectedMovementListData>> HandleAsync(GetRejectedMovements message)
        {
            var rejectedMovements = await repository.GetRejectedMovements(message.NotificationId);

            return rejectedMovements.Select(m => new RejectedMovementListData
            {
                Id = m.Id,
                Number = m.Number,
                PrenotificationDate = m.PrenotificationDate,
                ShipmentDate = m.Date
            }).ToList();
        }
    }
}
