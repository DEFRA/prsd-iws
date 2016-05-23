namespace EA.Iws.DataAccess.Security
{
    using System;
    using System.Threading.Tasks;
    using Domain.ImportMovement;
    using Domain.Security;

    internal class ImportMovementAuthorization : IImportMovementAuthorization
    {
        private readonly IImportNotificationApplicationAuthorization authorization;
        private readonly IImportMovementRepository repository;

        public ImportMovementAuthorization(IImportNotificationApplicationAuthorization authorization, IImportMovementRepository repository)
        {
            this.authorization = authorization;
            this.repository = repository;
        }

        public async Task EnsureAccessAsync(Guid movementId)
        {
            var notificationId = (await repository.Get(movementId)).NotificationId;
            await authorization.EnsureAccessAsync(notificationId);
        }
    }
}