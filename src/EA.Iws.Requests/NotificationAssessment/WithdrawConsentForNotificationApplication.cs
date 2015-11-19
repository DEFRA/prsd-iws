namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Prsd.Core.Mediator;

    public class WithdrawConsentForNotificationApplication : IRequest<bool>
    {
        public string ReasonsForConsentWithdrawal { get; private set; }

        public Guid Id { get; private set; }

        public WithdrawConsentForNotificationApplication(Guid id, string reasonsForConsentWithdrawal)
        {
            ReasonsForConsentWithdrawal = reasonsForConsentWithdrawal;
            Id = id;
        }
    }
}