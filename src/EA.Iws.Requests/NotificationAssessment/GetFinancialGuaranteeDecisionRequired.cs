namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotificationAssessment)]
    public class GetFinancialGuaranteeDecisionRequired : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }

        public GetFinancialGuaranteeDecisionRequired(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}