namespace EA.Iws.DocumentGeneration.Notification.Blocks.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.TransportRoute;
    using EA.Iws.Domain.NotificationApplication;    

    internal class CustomsOfficeBlockFactory : INotificationBlockFactory
    {
        private readonly ITransportRouteRepository transportRouteRepository;
        private readonly INotificationApplicationRepository notificationApplicationRepository;

        public CustomsOfficeBlockFactory(ITransportRouteRepository transportRouteRepository, INotificationApplicationRepository notificationApplicationRepository)
        {
            this.transportRouteRepository = transportRouteRepository;
            this.notificationApplicationRepository = notificationApplicationRepository;
        }

        public async Task<IDocumentBlock> Create(Guid notificationId, IList<MergeField> mergeFields)
        {
            var transportRoute = await transportRouteRepository.GetByNotificationId(notificationId);
            var notificationCompetenetAuthority = await notificationApplicationRepository.GetNotificationCompetentAuthority(notificationId);
            return new CustomsOfficeBlock(mergeFields, transportRoute, notificationCompetenetAuthority);
        }
    }
}