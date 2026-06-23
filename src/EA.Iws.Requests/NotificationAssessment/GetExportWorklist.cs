namespace EA.Iws.Requests.NotificationAssessment
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.NotificationAssessment;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotificationAssessment)]
    public class GetExportWorklist : IRequest<ExportWorklistResult>
    {
        public string NotificationNumber { get; set; }

        public string Officer { get; set; }

        public NotificationStatus? Status { get; set; }

        public int PageNumber { get; set; }
    }
}