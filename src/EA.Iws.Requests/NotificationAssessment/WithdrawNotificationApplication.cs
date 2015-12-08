namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Prsd.Core.Mediator;

    public class WithdrawNotificationApplication : IRequest<bool>
    {
        public Guid Id { get; private set; }

        public string Reason { get; private set; }

        public DateTime Date { get; private set; }

        public WithdrawNotificationApplication(Guid id, DateTime date, string reason)
        {
            Id = id;
            Date = date;
            Reason = reason;
        }
    }
}