namespace EA.Iws.RequestHandlers.Notification
{
    using System;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GetNotificationIdByNumberHandler : IRequestHandler<GetNotificationIdByNumber, Guid?>
    {
        private readonly INotificationApplicationRepository notificationApplicationRepository;
        private static readonly Regex NotificationNumberRegex = new Regex(@"(GB)(\d{4})(\d{6})", RegexOptions.Compiled);

        public GetNotificationIdByNumberHandler(INotificationApplicationRepository notificationApplicationRepository)
        {
            this.notificationApplicationRepository = notificationApplicationRepository;
        }

        public async Task<Guid?> HandleAsync(GetNotificationIdByNumber message)
        {
            return await notificationApplicationRepository.GetIdOrDefault(FormatNotificationNumber(message.NotificationNumber));
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
