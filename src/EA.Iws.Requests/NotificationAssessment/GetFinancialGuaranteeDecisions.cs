namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.NotificationAssessment;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotificationAssessment)]
    public class GetFinancialGuaranteeDecisions : IRequest<IEnumerable<FinancialGuaranteeDecisionData>>
    {
        public Guid NotificationId { get; private set; }

        public GetFinancialGuaranteeDecisions(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}