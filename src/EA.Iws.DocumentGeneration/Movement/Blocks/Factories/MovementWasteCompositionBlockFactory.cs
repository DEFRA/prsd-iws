namespace EA.Iws.DocumentGeneration.Movement.Blocks.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.NotificationApplication;

    internal class MovementWasteCompositionBlockFactory : IMovementBlockFactory
    {
        private readonly INotificationApplicationRepository notificationApplicationRepository;

        public MovementWasteCompositionBlockFactory(INotificationApplicationRepository notificationApplicationRepository)
        {
            this.notificationApplicationRepository = notificationApplicationRepository;
        }

        public async Task<IDocumentBlock> Create(Guid movementId, IList<MergeField> mergeFields)
        {
            var notification = await notificationApplicationRepository.GetByMovementId(movementId);
            return new MovementWasteCompositionBlock(mergeFields, notification);
        }
    }
}