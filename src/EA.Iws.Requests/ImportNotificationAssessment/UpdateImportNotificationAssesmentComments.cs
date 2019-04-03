namespace EA.Iws.Requests.ImportNotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanReadImportNotificationAssessment)]
    public class UpdateImportNotificationAssesmentComments : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }
        public string Comment { get; private set; }
        public UpdateImportNotificationAssesmentComments(Guid notificationId, string comment)
        {
            NotificationId = notificationId;
            Comment = comment;
        }
    }
}
