namespace EA.Iws.Requests.ImportNotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanEditImportNotificationAssessment)]
    public class WithdrawConsentForImportNotification : IRequest<bool>
    {
        public string ReasonsForConsentWithdrawal { get; private set; }

        public Guid Id { get; private set; }

        public DateTime Date { get; private set; }

        public WithdrawConsentForImportNotification(Guid id, string reasonsForConsentWithdrawal, DateTime date)
        {
            ReasonsForConsentWithdrawal = reasonsForConsentWithdrawal;
            Id = id;
            Date = date;
        }
    }
}
