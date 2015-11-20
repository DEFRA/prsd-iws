namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class MovementNumberValidator : IMovementNumberValidator
    {
        private readonly IMovementRepository repository;

        public MovementNumberValidator(IMovementRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Validate(Guid notificationId, int number)
        {
            var movements = await repository.GetAllMovements(notificationId);

            return movements.All(m => m.Number != number);
        }
    }
}
