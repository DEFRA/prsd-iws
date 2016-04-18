namespace EA.Iws.DocumentGeneration.Notification.Blocks.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.NotificationApplication;

    internal class FacilityBlockFactory : INotificationBlockFactory
    {
        private readonly IFacilityRepository facilityRepository;

        public FacilityBlockFactory(IFacilityRepository facilityRepository)
        {
            this.facilityRepository = facilityRepository;
        }

        public async Task<IDocumentBlock> Create(Guid notificationId, IList<MergeField> mergeFields)
        {
            var facilityCollection = await facilityRepository.GetByNotificationId(notificationId);
            return new FacilityBlock(mergeFields, facilityCollection);
        }
    }
}