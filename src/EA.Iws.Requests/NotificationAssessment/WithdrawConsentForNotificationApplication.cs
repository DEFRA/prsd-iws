namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanMakeExportNotificationAssessmentDecision)]
    public class WithdrawConsentForNotificationApplication : IRequest<bool>
    {
        public string ReasonsForConsentWithdrawal { get; private set; }

        public Guid Id { get; private set; }

        public DateTime Date { get; private set; }

        public WithdrawConsentForNotificationApplication(Guid id, string reasonsForConsentWithdrawal, DateTime date)
        {
            ReasonsForConsentWithdrawal = reasonsForConsentWithdrawal;
            Date = date;
            Id = id;
        }
    }
}