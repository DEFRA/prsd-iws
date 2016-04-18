namespace EA.Iws.RequestHandlers.Notification
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using Prsd.Core.Domain;

    public class SetChargeHandler : IEventHandler<NotificationTransmittedEvent>
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

        public async Task HandleAsync(NotificationTransmittedEvent @event)
        {
            await UpdateCharge(@event.NotificationApplicationId);
        }

        private async Task UpdateCharge(Guid notificationApplicationId)
        {
            var notification = await notificationRepository.GetById(notificationApplicationId);
            var charge = await chargeCalculator.GetCalculatedValue(notificationApplicationId);

            notification.SetCharge(charge);

            await context.SaveChangesAsync();
        }
    }
}
