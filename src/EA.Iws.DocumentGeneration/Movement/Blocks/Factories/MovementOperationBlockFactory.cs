namespace EA.Iws.DocumentGeneration.Movement.Blocks.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.NotificationApplication;

    internal class MovementOperationBlockFactory : IMovementBlockFactory
    {
        private readonly INotificationApplicationRepository notificationApplicationRepository;

        public MovementOperationBlockFactory(INotificationApplicationRepository notificationApplicationRepository)
        {
            this.notificationApplicationRepository = notificationApplicationRepository;
        }

        public async Task<IDocumentBlock> Create(Guid movementId, IList<MergeField> mergeFields)
        {
            var notification = await notificationApplicationRepository.GetByMovementId(movementId);
            return new MovementOperationBlock(mergeFields, notification);
        }
    }
}