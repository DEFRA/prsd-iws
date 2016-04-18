namespace EA.Iws.DataAccess.Security
{
    using System;
    using System.Threading.Tasks;
    using Domain.Movement;
    using Domain.Security;

    internal class MovementAuthorization : IMovementAuthorization
    {
        private readonly IMovementRepository repository;
        private readonly INotificationApplicationAuthorization notificationAuthorization;

        public MovementAuthorization(INotificationApplicationAuthorization notificationAuthorization,
            IMovementRepository repository)
        {
            this.notificationAuthorization = notificationAuthorization;
            this.repository = repository;
        }

        public async Task EnsureAccessAsync(Guid movementId)
        {
            var notificationId = (await repository.GetById(movementId)).NotificationId;

            await notificationAuthorization.EnsureAccessAsync(notificationId);
        }
    }
}