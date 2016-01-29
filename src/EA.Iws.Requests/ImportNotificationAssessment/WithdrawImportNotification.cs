namespace EA.Iws.Requests.ImportNotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanEditImportNotificationAssessment)]
    public class WithdrawImportNotification : IRequest<bool>
    {
        public Guid ImportNotificationId { get; private set; }

        public DateTime Date { get; private set; }

        public string Reasons { get; private set; }

        public WithdrawImportNotification(Guid importNotificationId, string reasons, DateTime date)
        {
            ImportNotificationId = importNotificationId;
            Date = date;
            Reasons = reasons;
        }
    }
}
