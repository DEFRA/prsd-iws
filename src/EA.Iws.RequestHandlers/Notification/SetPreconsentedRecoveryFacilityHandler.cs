namespace EA.Iws.RequestHandlers.Notification
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    public class SetPreconsentedRecoveryFacilityHandler : IRequestHandler<SetPreconsentedRecoveryFacility, string>
    {
        private readonly IwsContext context;
        private readonly INotificationApplicationRepository notificationApplicationRepository;
        private readonly SetAllFacilitiesPreconsented setAllFacilitiesPreconsented;

        public SetPreconsentedRecoveryFacilityHandler(IwsContext context,
            INotificationApplicationRepository notificationApplicationRepository,
            SetAllFacilitiesPreconsented setAllFacilitiesPreconsented)
        {
            this.context = context;
            this.notificationApplicationRepository = notificationApplicationRepository;
            this.setAllFacilitiesPreconsented = setAllFacilitiesPreconsented;
        }

        public async Task<string> HandleAsync(SetPreconsentedRecoveryFacility query)
        {
            var notification = await notificationApplicationRepository.GetById(query.NotificationId);
            await setAllFacilitiesPreconsented.SetForNotification(notification, query.IsPreconsentedRecoveryFacility);

            await context.SaveChangesAsync();

            return notification.NotificationNumber;
        }
    }
}