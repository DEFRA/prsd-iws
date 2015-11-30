namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class OriginalMovementDate
    {
        private readonly IMovementDateHistoryRepository historyRepository;

        public OriginalMovementDate(IMovementDateHistoryRepository historyRepository)
        {
            this.historyRepository = historyRepository;
        }

        public async Task<DateTime> Get(Movement movement)
        {
            var previousDates = await historyRepository.GetByMovementId(movement.Id);

            if (!previousDates.Any())
            {
                return movement.Date;
            }

            return previousDates
                .OrderBy(d => d.DateChanged)
                .Select(d => d.PreviousDate)
                .First();
        }
    }
}