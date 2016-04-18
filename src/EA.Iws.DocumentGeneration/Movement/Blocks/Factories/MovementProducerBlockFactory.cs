namespace EA.Iws.DocumentGeneration.Movement.Blocks.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.NotificationApplication;

    internal class MovementProducerBlockFactory : IMovementBlockFactory
    {
        private readonly IProducerRepository producerRepository;
        private readonly INotificationApplicationRepository notificationApplicationRepository;

        public MovementProducerBlockFactory(INotificationApplicationRepository notificationApplicationRepository,
            IProducerRepository producerRepository)
        {
            this.notificationApplicationRepository = notificationApplicationRepository;
            this.producerRepository = producerRepository;
        }

        public async Task<IDocumentBlock> Create(Guid movementId, IList<MergeField> mergeFields)
        {
            var notification = await notificationApplicationRepository.GetByMovementId(movementId);
            var producer = await producerRepository.GetByMovementId(movementId);

            return new MovementProducerBlock(mergeFields, notification, producer);
        }
    }
}