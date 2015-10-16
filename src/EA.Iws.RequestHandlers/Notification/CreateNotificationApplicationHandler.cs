namespace EA.Iws.RequestHandlers.Notification
{
    using System;
    using System.Threading.Tasks;
    using Core.Shared;
    using DataAccess;
    using Domain;
    using Domain.NotificationApplication;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using CompetentAuthority = Core.Notification.CompetentAuthority;

    internal class CreateNotificationApplicationHandler : IRequestHandler<CreateNotificationApplication, Guid>
    {
        private readonly IwsContext context;
        private readonly INotificationNumberGenerator notificationNumberGenerator;
        private readonly IUserContext userContext;

        public CreateNotificationApplicationHandler(IwsContext context, IUserContext userContext,
            INotificationNumberGenerator notificationNumberGenerator)
        {
            this.context = context;
            this.userContext = userContext;
            this.notificationNumberGenerator = notificationNumberGenerator;
        }

        public async Task<Guid> HandleAsync(CreateNotificationApplication command)
        {
            var authority = GetUkCompetentAuthority(command.CompetentAuthority);
            var notificationType = GetNotificationType(command.NotificationType);

            var notificationNumber = await notificationNumberGenerator.GetNextNotificationNumber(authority);
            var notification = new NotificationApplication(userContext.UserId, notificationType, authority,
                notificationNumber);

            context.NotificationApplications.Add(notification);
            await context.SaveChangesAsync();

            return notification.Id;
        }

        private static NotificationType GetNotificationType(Core.Shared.NotificationType notificationType)
        {
            NotificationType type;
            switch (notificationType)
            {
                case Core.Shared.NotificationType.Recovery:
                    type = NotificationType.Recovery;
                    break;
                case Core.Shared.NotificationType.Disposal:
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