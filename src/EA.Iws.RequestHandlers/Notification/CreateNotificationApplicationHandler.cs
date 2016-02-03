namespace EA.Iws.RequestHandlers.Notification
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class CreateNotificationApplicationHandler : IRequestHandler<CreateNotificationApplication, Guid>
    {
        private readonly IProducerRepository producerRepository;
        private readonly ICarrierRepository carrierRepository;
        private readonly NotificationApplicationFactory notificationApplicationFactory;
        private readonly IwsContext context;
        private readonly INotificationApplicationRepository notificationApplicationRepository;
        private readonly IFacilityRepository facilityRepository;

        public CreateNotificationApplicationHandler(
            NotificationApplicationFactory notificationApplicationFactory,
            IwsContext context,
            INotificationApplicationRepository notificationApplicationRepository,
            IFacilityRepository facilityRepository,
            ICarrierRepository carrierRepository,
            IProducerRepository producerRepository)
        {
            this.notificationApplicationFactory = notificationApplicationFactory;
            this.context = context;
            this.notificationApplicationRepository = notificationApplicationRepository;
            this.facilityRepository = facilityRepository;
            this.carrierRepository = carrierRepository;
            this.producerRepository = producerRepository;
        }

        public async Task<Guid> HandleAsync(CreateNotificationApplication command)
        {
            var authority = UKCompetentAuthority.FromCompetentAuthority(command.CompetentAuthority);

            var notification = await notificationApplicationFactory.CreateNew(command.NotificationType, authority);

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