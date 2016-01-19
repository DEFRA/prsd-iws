namespace EA.Iws.Requests.ImportNotificationAssessment
{
    using System;
    using Prsd.Core.Mediator;

    public class WithdrawConsentForImportNotification : IRequest<bool>
    {
        public string ReasonsForConsentWithdrawal { get; private set; }

        public Guid Id { get; private set; }

        public WithdrawConsentForImportNotification(Guid id, string reasonsForConsentWithdrawal)
        {
            ReasonsForConsentWithdrawal = reasonsForConsentWithdrawal;
            Id = id;
        }
    }
}
