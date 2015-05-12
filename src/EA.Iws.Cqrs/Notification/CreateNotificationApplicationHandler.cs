namespace EA.Iws.Cqrs.Notification
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.Notification;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;

    public class CreateNotificationApplicationHandler : IRequestHandler<CreateNotificationApplication, Guid>
    {
        private const string NotificationNumberSequenceFormat = "[Notification].[{0}NotificationNumber]";

        private readonly IwsContext context;
        private readonly IUserContext userContext;

        public CreateNotificationApplicationHandler(IwsContext context, IUserContext userContext)
        {
            this.context = context;
            this.userContext = userContext;
        }

        public async Task<Guid> HandleAsync(CreateNotificationApplication command)
        {
            UKCompetentAuthority authority;
            Domain.Notification.WasteAction wasteAction;

            switch (command.CompetentAuthority)
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
                    throw new InvalidOperationException(string.Format("Unknown competent authority: {0}", command.CompetentAuthority));
            }

            switch (command.WasteAction)
            {
                case WasteAction.Recovery:
                    wasteAction = Domain.Notification.WasteAction.Recovery;
                    break;
                case WasteAction.Disposal:
                    wasteAction = Domain.Notification.WasteAction.Disposal;
                    break;
                default:
                    throw new InvalidOperationException(string.Format("Unknown waste action: {0}", command.WasteAction));
            }

            var notificationNumber = await GetNextNotificationNumberAsync(authority);
            var notification = new NotificationApplication(userContext.UserId, wasteAction, authority, notificationNumber);
            context.NotificationApplications.Add(notification);
            await context.SaveChangesAsync();
            return notification.Id;
        }

        private async Task<int> GetNextNotificationNumberAsync(UKCompetentAuthority authority)
        {
            return await context.Database.SqlQuery<int>(CreateSqlQuery(authority)).SingleAsync();
        }

        private static string CreateSqlQuery(UKCompetentAuthority competentAuthority)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT NEXT VALUE FOR ");
            stringBuilder.AppendFormat(NotificationNumberSequenceFormat, competentAuthority.ShortName);
            return stringBuilder.ToString();
        }
    }
}