namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Threading.Tasks;

    public class CapturedMovementFactory : ICapturedMovementFactory
    {
        private readonly IMovementNumberValidator movementNumberValidator;

        public CapturedMovementFactory(IMovementNumberValidator movementNumberValidator)
        {
            this.movementNumberValidator = movementNumberValidator;
        }

        public async Task<Movement> Create(Guid notificationId, int number, DateTime? prenotificationDate, DateTime actualShipmentDate)
        {
            if (!await movementNumberValidator.Validate(notificationId, number))
            {
                throw new MovementNumberException("Cannot create a movement with a conflicting movement number (" + number + ") for notification: " + notificationId);
            }

            var movement = Movement.Capture(number, notificationId, actualShipmentDate, prenotificationDate);

            return movement;
        }
    }
}