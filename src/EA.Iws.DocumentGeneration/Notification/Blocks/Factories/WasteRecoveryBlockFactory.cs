namespace EA.Iws.DocumentGeneration.Notification.Blocks.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.WasteRecovery;

    internal class WasteRecoveryBlockFactory : INotificationBlockFactory
    {
        private readonly INotificationApplicationRepository notificationApplicationRepository;
        private readonly IWasteDisposalRepository wasteDisposalRepository;
        private readonly IWasteRecoveryRepository wasteRecoveryRepository;

        public WasteRecoveryBlockFactory(INotificationApplicationRepository notificationApplicationRepository,
            IWasteRecoveryRepository wasteRecoveryRepository,
            IWasteDisposalRepository wasteDisposalRepository)
        {
            this.notificationApplicationRepository = notificationApplicationRepository;
            this.wasteRecoveryRepository = wasteRecoveryRepository;
            this.wasteDisposalRepository = wasteDisposalRepository;
        }

        public async Task<IDocumentBlock> Create(Guid notificationId, IList<MergeField> mergeFields)
        {
            var notification = await notificationApplicationRepository.GetById(notificationId);
            var wasteRecovery = await wasteRecoveryRepository.GetByNotificationId(notificationId);
            var wasteDisposal = await wasteDisposalRepository.GetByNotificationId(notificationId);

            return new WasteRecoveryBlock(mergeFields, notification, wasteRecovery, wasteDisposal);
        }
    }
}