namespace EA.Iws.Cqrs.Notification
{
    using System.Threading.Tasks;
    using Core.Cqrs;
    using Core.Domain;
    using DataAccess;
    using Domain.Notification;

    public class CreateNotificationApplicationHandler : ICommandHandler<CreateNotificationApplication>
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;
        private readonly IQueryBus queryBus;

        public CreateNotificationApplicationHandler(IwsContext context, IUserContext userContext, IQueryBus queryBus)
        {
            this.context = context;
            this.userContext = userContext;
            this.queryBus = queryBus;
        }

        public async Task HandleAsync(CreateNotificationApplication command)
        {
            var notificationNumber =
                await queryBus.QueryAsync(new GetNextNotificationNumber(command.CompetentAuthority));
            var notification = new NotificationApplication(userContext.UserId, command.WasteAction, command.CompetentAuthority, notificationNumber);
            context.NotificationApplications.Add(notification);
            await context.SaveChangesAsync();
            command.NotificationId = notification.Id;
        }
    }
}