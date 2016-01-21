namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.NotificationAssessment;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotificationAssessment)]
    public class GetAccountManagementData : IRequest<AccountManagementData>
    {
        public GetAccountManagementData(Guid id)
        {
            NotificationId = id;
        }

        public Guid NotificationId;
    }
}
