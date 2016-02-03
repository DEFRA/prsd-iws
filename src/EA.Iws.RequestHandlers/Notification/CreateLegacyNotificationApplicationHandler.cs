namespace EA.Iws.RequestHandlers.Notification
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class CreateLegacyNotificationApplicationHandler :
        IRequestHandler<CreateLegacyNotificationApplication, Guid>
    {
        private readonly IwsContext context;
        private readonly NotificationApplicationFactory notificationApplicationFactory;
        private readonly INotificationApplicationRepository notificationApplicationRepository;
        private readonly IFacilityRepository facilityRepository;
        private readonly ICarrierRepository carrierRepository;
        private readonly IProducerRepository producerRepository;

        public CreateLegacyNotificationApplicationHandler(IwsContext context,
            INotificationApplicationRepository notificationApplicationRepository,
            IFacilityRepository facilityRepository,
            ICarrierRepository carrierRepository,
            IProducerRepository producerRepository,
            NotificationApplicationFactory notificationApplicationFactory)
        {
            this.context = context;
            this.notificationApplicationRepository = notificationApplicationRepository;
            this.facilityRepository = facilityRepository;
            this.carrierRepository = carrierRepository;
            this.producerRepository = producerRepository;
            this.notificationApplicationFactory = notificationApplicationFactory;
        }

        public async Task<Guid> HandleAsync(CreateLegacyNotificationApplication message)
        {
            var notification =
                await
                    notificationApplicationFactory.CreateLegacy(
                        message.NotificationType,
                        UKCompetentAuthority.FromCompetentAuthority(message.CompetentAuthority), 
                        message.Number);

            notificationApplicationRepository.Add(notification);

            await context.SaveChangesAsync();

            var facilityCollection = new FacilityCollection(notification.Id);
            var carrierCollection = new CarrierCollection(notification.Id);
            var producerCollection = new ProducerCollection(notification.Id);

            facilityRepository.Add(facilityCollection);
            carrierRepository.Add(carrierCollection);
            producerRepository.Add(producerCollection);

            await context.SaveChangesAsync();

            return notification.Id;
        }
    }
}