namespace EA.Iws.Requests.ImportNotificationAssessment
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.ImportNotificationAssessment;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanReadImportNotificationAssessment)]
    public class GetImportWorklist : IRequest<ImportWorklistResult>
    {
        public string NotificationNumber { get; set; }
        public string Officer { get; set; }
        public ImportNotificationStatus[] Statuses { get; set; }
        public int PageNumber { get; set; }
    }
}