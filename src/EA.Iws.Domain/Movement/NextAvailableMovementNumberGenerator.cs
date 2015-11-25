namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class NextAvailableMovementNumberGenerator : INextAvailableMovementNumberGenerator
    {
        private readonly IMovementRepository movementRepository;

        public NextAvailableMovementNumberGenerator(IMovementRepository movementRepository)
        {
            this.movementRepository = movementRepository;
        }

        public async Task<int> GetNext(Guid notificationId)
        {
            var movements = await movementRepository.GetAllMovements(notificationId);

            if (!movements.Any())
            {
                return 1;
            }

            var movementNumbers = movements.Select(m => m.Number).ToArray();

            return Enumerable.Range(1, movementNumbers.Max() + 1).Where(i => !movementNumbers.Contains(i)).Min();
        }
    }
}