namespace EA.Iws.Requests.ImportNotificationAssessment.Transactions
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Mediator;

    public class AddNotificationRefund : IRequest<bool>, IRequest
    {
        public AddNotificationRefund(Guid importNotificationId, decimal amount, DateTime date, string comments)
        {
            Guard.ArgumentNotDefaultValue(() => importNotificationId, importNotificationId);

            ImportNotificationId = importNotificationId;
            Amount = amount;
            Date = date;
            Comments = comments;
        }

        public Guid ImportNotificationId { get; private set; }

        public decimal Amount { get; private set; }

        public DateTime Date { get; private set; }

        public string Comments { get; private set; }
    }
}