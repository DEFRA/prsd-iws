namespace EA.Iws.RequestHandlers.Movement
{
    using System.Threading.Tasks;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class GetNotificationNumberByMovementIdHandler : IRequestHandler<GetNotificationNumberByMovementId, string>
    {
        private readonly INotificationApplicationRepository repository;

        public GetNotificationNumberByMovementIdHandler(INotificationApplicationRepository repository)
        {
            this.repository = repository;
        }

        public async Task<string> HandleAsync(GetNotificationNumberByMovementId message)
        {
            var notification = await repository.GetByMovementId(message.MovementId);
            return notification.NotificationNumber;
        }
    }
}