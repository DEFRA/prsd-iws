namespace EA.Iws.DocumentGeneration.Movement.Blocks.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.NotificationApplication;

    internal class MovementOperationBlockFactory : IMovementBlockFactory
    {
        private readonly INotificationApplicationRepository notificationApplicationRepository;
        private readonly ITechnologyEmployedRepository technologyEmployedRepository;

        public MovementOperationBlockFactory(INotificationApplicationRepository notificationApplicationRepository,
            ITechnologyEmployedRepository technologyEmployedRepository)
        {
            this.notificationApplicationRepository = notificationApplicationRepository;
            this.technologyEmployedRepository = technologyEmployedRepository;
        }

        public async Task<IDocumentBlock> Create(Guid movementId, IList<MergeField> mergeFields)
        {
            var notification = await notificationApplicationRepository.GetByMovementId(movementId);
            var technologyEmployed = await technologyEmployedRepository.GetByNotificaitonId(notification.Id);
            return new MovementOperationBlock(mergeFields, notification, technologyEmployed);
        }
    }
}