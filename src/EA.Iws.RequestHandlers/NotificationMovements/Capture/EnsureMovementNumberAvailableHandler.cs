namespace EA.Iws.RequestHandlers.NotificationMovements.Capture
{
    using System.Threading.Tasks;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.Capture;

    public class EnsureMovementNumberAvailableHandler : IRequestHandler<EnsureMovementNumberAvailable, bool>
    {
        private readonly IMovementNumberValidator numberValidator;

        public EnsureMovementNumberAvailableHandler(IMovementNumberValidator numberValidator)
        {
            this.numberValidator = numberValidator;
        }

        public async Task<bool> HandleAsync(EnsureMovementNumberAvailable message)
        {
            return await numberValidator.Validate(message.NotificationId, message.Number);
        }
    }
}