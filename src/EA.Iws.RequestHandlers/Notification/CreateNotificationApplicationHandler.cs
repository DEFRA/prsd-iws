namespace EA.Iws.RequestHandlers.Notification
{
    using System;
    using System.Threading.Tasks;
    using Core.Shared;
    using DataAccess;
    using Domain;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using CompetentAuthority = Core.Notification.CompetentAuthority;

    internal class CreateNotificationApplicationHandler : IRequestHandler<CreateNotificationApplication, Guid>
    {
        private readonly IProducerRepository producerRepository;
        private readonly ICarrierRepository carrierRepository;
        private readonly NotificationApplicationFactory notificationApplicationFactory;
        private readonly IwsContext context;
        private readonly IFacilityRepository facilityRepository;

        public CreateNotificationApplicationHandler(
            NotificationApplicationFactory notificationApplicationFactory,
            IwsContext context,
            IFacilityRepository facilityRepository,
            ICarrierRepository carrierRepository,
            IProducerRepository producerRepository)
        {
            this.notificationApplicationFactory = notificationApplicationFactory;
            this.context = context;
            this.facilityRepository = facilityRepository;
            this.carrierRepository = carrierRepository;
            this.producerRepository = producerRepository;
        }

        public async Task<Guid> HandleAsync(CreateNotificationApplication command)
        {
            var authority = GetUkCompetentAuthority(command.CompetentAuthority);
            var notificationType = GetNotificationType(command.NotificationType);

            var notification = await notificationApplicationFactory.CreateNew(notificationType, authority);

            context.NotificationApplications.Add(notification);

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

        private static NotificationType GetNotificationType(NotificationType notificationType)
        {
            NotificationType type;
            switch (notificationType)
            {
                case NotificationType.Recovery:
                    type = NotificationType.Recovery;
                    break;
                case NotificationType.Disposal:
                    type = NotificationType.Disposal;
                    break;
                default:
                    throw new InvalidOperationException(string.Format("Unknown notification type: {0}",
                        notificationType));
            }
            return type;
        }

        private static UKCompetentAuthority GetUkCompetentAuthority(CompetentAuthority competentAuthority)
        {
            UKCompetentAuthority authority;
            switch (competentAuthority)
            {
                case CompetentAuthority.England:
                    authority = UKCompetentAuthority.England;
                    break;
                case CompetentAuthority.NorthernIreland:
                    authority = UKCompetentAuthority.NorthernIreland;
                    break;
                case CompetentAuthority.Scotland:
                    authority = UKCompetentAuthority.Scotland;
                    break;
                case CompetentAuthority.Wales:
                    authority = UKCompetentAuthority.Wales;
                    break;
                default:
                    throw new InvalidOperationException(string.Format("Unknown competent authority: {0}",
                        competentAuthority));
            }
            return authority;
        }
    }
}