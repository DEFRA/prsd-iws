namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.NotificationAssessment;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotificationAssessment)]
    public class UpdateExportNotificationAssementComments : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }
        public string Comment { get; private set; }
        public UpdateExportNotificationAssementComments(Guid notificationId, string comment)
        {
            NotificationId = notificationId;
            Comment = comment;
        }
    }
}
