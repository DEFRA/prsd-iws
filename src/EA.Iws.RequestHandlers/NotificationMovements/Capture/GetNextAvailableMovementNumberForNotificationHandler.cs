namespace EA.Iws.RequestHandlers.NotificationMovements.Capture
{
    using System.Threading.Tasks;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.Capture;

    internal class GetNextAvailableMovementNumberForNotificationHandler : IRequestHandler<GetNextAvailableMovementNumberForNotification, int>
    {
        private readonly IMovementNumberGenerator numberGenerator;

        public GetNextAvailableMovementNumberForNotificationHandler(IMovementNumberGenerator numberGenerator)
        {
            this.numberGenerator = numberGenerator;
        }

        public async Task<int> HandleAsync(GetNextAvailableMovementNumberForNotification message)
        {
            return await numberGenerator.Generate(message.Id);
        }
    }
}
