namespace EA.Iws.Requests.Admin.FinancialGuarantee
{
    using System;
    using Core.Admin;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotificationAssessment)]
    public class GetFinancialGuaranteeDataByNotificationApplicationId : IRequest<FinancialGuaranteeData>
    {
        public Guid Id { get; private set; }

        public GetFinancialGuaranteeDataByNotificationApplicationId(Guid id)
        {
            Id = id;
        }
    }
}
