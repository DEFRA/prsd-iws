namespace EA.Iws.DocumentGeneration.Notification.Blocks.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.TransportRoute;

    internal class TransportBlockFactory : INotificationBlockFactory
    {
        private readonly ITransportRouteRepository transportRouteRepository;

        public TransportBlockFactory(ITransportRouteRepository transportRouteRepository)
        {
            this.transportRouteRepository = transportRouteRepository;
        }

        public async Task<IDocumentBlock> Create(Guid notificationId, IList<MergeField> mergeFields)
        {
            var transportRoute = await transportRouteRepository.GetByNotificationId(notificationId);
            return new TransportBlock(mergeFields, transportRoute);
        }
    }
}