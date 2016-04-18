namespace EA.Iws.Requests.ImportNotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.ImportNotificationAssessment;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanReadImportNotificationAssessment)]
    public class GetImportNotificationConsultation : IRequest<ConsultationData>
    {
        public Guid NotificationId { get; private set; }

        public GetImportNotificationConsultation(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
