namespace EA.Iws.RequestHandlers.Notification
{
    using EA.Iws.Core.Notification;
    using EA.Iws.Domain.NotificationApplication;
    using EA.Iws.Requests.Notification;
    using EA.Prsd.Core.Mediator;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    internal class GetExportNotificationIdHandler : IRequestHandler<GetExportNotificationId, DeleteExportNotificationDetails>
    {
        private readonly INotificationApplicationRepository notificationApplicationRepository;
        private static readonly Regex NotificationNumberRegex = new Regex(@"(GB)(\d{4})(\d{6})", RegexOptions.Compiled);

        public GetExportNotificationIdHandler(INotificationApplicationRepository notificationApplicationRepository)
        {
            this.notificationApplicationRepository = notificationApplicationRepository;
        }

        public async Task<DeleteExportNotificationDetails> HandleAsync(GetExportNotificationId getExportNotification)
        {
            string notificationNumber = FormatNotificationNumber(getExportNotification.NotificationNumber);
            DeleteExportNotificationDetails deleteExportNotificationDetails = await notificationApplicationRepository.ValidateExportNotification(notificationNumber);

            return deleteExportNotificationDetails;
        }

        private static string FormatNotificationNumber(string notificationNumber)
        {
            if (string.IsNullOrWhiteSpace(notificationNumber))
            {
                return string.Empty;
            }

            notificationNumber = notificationNumber.ToUpper().Replace(" ", string.Empty);

            if (NotificationNumberRegex.IsMatch(notificationNumber))
            {
                notificationNumber = NotificationNumberRegex.Replace(notificationNumber, "$1 $2 $3");
            }

            return notificationNumber;
        }
    }
}
