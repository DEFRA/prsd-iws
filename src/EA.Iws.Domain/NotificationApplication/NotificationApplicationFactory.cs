namespace EA.Iws.Domain.NotificationApplication
{
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using Core.Shared;
    using Prsd.Core.Domain;

    [AutoRegister]
    public class NotificationApplicationFactory
    {
        private readonly IUserContext userContext;
        private readonly INotificationNumberGenerator numberGenerator;

        public NotificationApplicationFactory(IUserContext userContext, INotificationNumberGenerator numberGenerator)
        {
            this.userContext = userContext;
            this.numberGenerator = numberGenerator;
        }

        public Task<NotificationApplication> CreateLegacy(NotificationType notificationType, UKCompetentAuthority competentAuthority, int number)
        {
            var notification = new NotificationApplication(userContext.UserId, notificationType, competentAuthority, number);
            return Task.FromResult(notification);
        }

        public async Task<NotificationApplication> CreateNew(NotificationType notificationType,
            UKCompetentAuthority competentAuthority)
        {
            var nextNotificationNumber = await numberGenerator.GetNextNotificationNumber(competentAuthority);
            var notification = new NotificationApplication(userContext.UserId, notificationType, competentAuthority, nextNotificationNumber);
            return notification;
        }
    }
}