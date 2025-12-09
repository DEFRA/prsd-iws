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

        public async Task<DeleteExportNotificationDetails> HandleAsync(GetExportNotificationId message)
        {
            var deleteExportNotificationDetails = await notificationApplicationRepository.ValidateExportNotification(FormatNotificationNumber(message.NotificationNumber));

            return deleteExportNotificationDetails;
        }

        private static string FormatNotificationNumber(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                return string.Empty;
            }

            number = number.ToUpper().Replace(" ", string.Empty);

            if (NotificationNumberRegex.IsMatch(number))
            {
                number = NotificationNumberRegex.Replace(number, "$1 $2 $3");
            }

            return number;
        }
    }
}
