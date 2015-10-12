namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Prsd.Core.Mediator;

    public class WithdrawNotificationApplication : IRequest<bool>
    {
        public Guid Id { get; private set; }

        public WithdrawNotificationApplication(Guid id)
        {
            Id = id;
        }
    }
}