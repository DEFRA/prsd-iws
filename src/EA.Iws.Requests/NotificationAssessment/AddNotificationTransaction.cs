namespace EA.Iws.Requests.NotificationAssessment
{
    using Core.NotificationAssessment;
    using Prsd.Core.Mediator;

    public class AddNotificationTransaction : IRequest<bool>
    {
        public AddNotificationTransaction(NotificationTransactionData data)
        {
            Data = data;
        }

        public NotificationTransactionData Data;
    }
}
