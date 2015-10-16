namespace EA.Iws.Requests.ImportNotification
{
    using Prsd.Core.Mediator;

    public class CheckImportNumberUnique : IRequest<bool>
    {
        public string NotificationNumber { get; private set; }

        public CheckImportNumberUnique(string notificationNumber)
        {
            NotificationNumber = notificationNumber;
        }
    }
}
