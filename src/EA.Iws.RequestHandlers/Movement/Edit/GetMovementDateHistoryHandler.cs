namespace EA.Iws.RequestHandlers.Movement.Edit
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.Movement;
    using Prsd.Core;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Movement.Edit;

    internal class GetMovementDateHistoryHandler : IRequestHandler<GetMovementDateHistory, IList<DateHistory>>
    {
        private readonly IMovementRepository movementRepository;
        private readonly IMapper mapper;
        private readonly IMovementDateHistoryRepository historyRepository;

        public GetMovementDateHistoryHandler(IMovementDateHistoryRepository historyRepository,
            IMovementRepository movementRepository,
            IMapper mapper)
        {
            this.historyRepository = historyRepository;
            this.mapper = mapper;
            this.movementRepository = movementRepository;
        }

        public async Task<IList<DateHistory>> HandleAsync(GetMovementDateHistory message)
        {
            var movement = await movementRepository.GetById(message.MovementId);
            var dateHistory = await historyRepository.GetByMovementId(message.MovementId);

            var dateHistoryList = dateHistory.Select(dh => mapper.Map<DateHistory>(dh)).ToList();

            dateHistoryList.Add(new DateHistory { PreviousDate = movement.Date, DateChanged = SystemTime.UtcNow });

            return dateHistoryList.OrderBy(l => l.DateChanged).ToList();
        }
    }
}