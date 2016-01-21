namespace EA.Iws.Requests.Admin.FinancialGuarantee
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotificationAssessment)]
    public class SetFinancialGuaranteeDates : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }

        public DateTime? ReceivedDate { get; private set; }

        public DateTime? CompletedDate { get; private set; }

        public SetFinancialGuaranteeDates(Guid notificationId, DateTime? receivedDate, DateTime? completedDate)
        {
            if (!receivedDate.HasValue && !completedDate.HasValue)
            {
                throw new ArgumentException("Either received date or completed date must be non-null");
            }

            NotificationId = notificationId;
            ReceivedDate = receivedDate;
            CompletedDate = completedDate;
        }
    }
}
