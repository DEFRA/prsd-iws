namespace EA.Iws.RequestHandlers.NotificationMovements.Capture
{
    using System.Threading.Tasks;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.Capture;

    internal class GetNextAvailableMovementNumberForNotificationHandler : IRequestHandler<GetNextAvailableMovementNumberForNotification, int>
    {
        private readonly INextAvailableMovementNumberGenerator nextAvailableMovementNumberGenerator;

        public GetNextAvailableMovementNumberForNotificationHandler(INextAvailableMovementNumberGenerator nextAvailableMovementNumberGenerator)
        {
            this.nextAvailableMovementNumberGenerator = nextAvailableMovementNumberGenerator;
        }

        public async Task<int> HandleAsync(GetNextAvailableMovementNumberForNotification message)
        {
            return await nextAvailableMovementNumberGenerator.GetNext(message.Id);
        }
    }
}
