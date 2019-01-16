namespace EA.Iws.RequestHandlers.Notification
{
    using System;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Domain.NotificationApplication;
    using Domain.Security;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GetNotificationIdByNumberHandler : IRequestHandler<GetNotificationIdByNumber, Guid?>
    {
        private readonly INotificationApplicationRepository notificationApplicationRepository;
        private readonly INotificationApplicationAuthorization notificationApplicationAuthorization;
        private static readonly Regex NotificationNumberRegex = new Regex(@"(GB)(\d{4})(\d{6})", RegexOptions.Compiled);

        public GetNotificationIdByNumberHandler(INotificationApplicationRepository notificationApplicationRepository, 
            INotificationApplicationAuthorization notificationApplicationAuthorization)
        {
            this.notificationApplicationRepository = notificationApplicationRepository;
            this.notificationApplicationAuthorization = notificationApplicationAuthorization;
        }

        public async Task<Guid?> HandleAsync(GetNotificationIdByNumber message)
        {
            var id =
                await
                    notificationApplicationRepository.GetIdOrDefault(FormatNotificationNumber(message.NotificationNumber));

            if (id != null)
            {
                await notificationApplicationAuthorization.EnsureAccessAsync(id.Value);
            }

            return id;
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
