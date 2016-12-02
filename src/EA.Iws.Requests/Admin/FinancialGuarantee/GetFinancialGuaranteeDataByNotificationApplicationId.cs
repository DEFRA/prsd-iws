namespace EA.Iws.Requests.Admin.FinancialGuarantee
{
    using System;
    using Core.Admin;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.FinancialGuarantee;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotificationAssessment)]
    public class GetFinancialGuaranteeDataByNotificationApplicationId : IRequest<FinancialGuaranteeData>
    {
        public Guid NotificationId { get; private set; }

        public Guid FinancialGuaranteeId { get; private set; }

        public GetFinancialGuaranteeDataByNotificationApplicationId(Guid notificationId, Guid financialGuaranteeId)
        {
            NotificationId = notificationId;
            FinancialGuaranteeId = financialGuaranteeId;
        }
    }
}
