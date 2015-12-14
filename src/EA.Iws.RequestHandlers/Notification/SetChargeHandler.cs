namespace EA.Iws.RequestHandlers.Notification
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using Prsd.Core.Domain;

    public class SetChargeHandler : IEventHandler<NotificationSubmittedEvent>, IEventHandler<NotificationTransmittedEvent>
    {
        private readonly IwsContext context;
        private readonly INotificationChargeCalculator chargeCalculator;
        private readonly INotificationApplicationRepository notificationRepository;

        public SetChargeHandler(IwsContext context, INotificationChargeCalculator chargeCalculator, INotificationApplicationRepository notificationRepository)
        {
            this.context = context;
            this.chargeCalculator = chargeCalculator;
            this.notificationRepository = notificationRepository;
        }

        public async Task HandleAsync(NotificationSubmittedEvent @event)
        {
            var notification = await notificationRepository.GetById(@event.NotificationApplicationId);
            var charge = await chargeCalculator.GetValue(@event.NotificationApplicationId);

            notification.SetCharge(charge);

            await context.SaveChangesAsync();
        }

        public async Task HandleAsync(NotificationTransmittedEvent @event)
        {
            var notification = await notificationRepository.GetById(@event.NotificationApplicationId);
            var charge = await chargeCalculator.GetCalculatedValue(@event.NotificationApplicationId);

            notification.SetCharge(charge);

            await context.SaveChangesAsync();
        }
    }
}
