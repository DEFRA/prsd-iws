namespace EA.Iws.RequestHandlers.ImportNotificationMovements
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.ImportMovement;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationMovements;

    internal class GetRejectedImportMovementsHandler : IRequestHandler<GetRejectedImportMovements, IList<RejectedMovementListData>>
    {
        private readonly IImportMovementRepository repository;

        public GetRejectedImportMovementsHandler(IImportMovementRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IList<RejectedMovementListData>> HandleAsync(GetRejectedImportMovements message)
        {
            var rejectedMovements = await repository.GetRejectedMovements(message.NotificationId);

            return rejectedMovements.Select(m => new RejectedMovementListData
            {
                Id = m.Id,
                Number = m.Number,
                PrenotificationDate = m.PrenotificationDate,
                ShipmentDate = m.ActualShipmentDate
            }).ToList();
        }
    }
}
