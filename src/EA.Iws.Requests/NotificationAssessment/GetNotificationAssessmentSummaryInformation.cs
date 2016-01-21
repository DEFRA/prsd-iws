namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.NotificationAssessment;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotificationAssessment)]
    public class GetNotificationAssessmentSummaryInformation : IRequest<NotificationAssessmentSummaryInformationData>
    {
        public Guid Id { get; private set; }

        public GetNotificationAssessmentSummaryInformation(Guid id)
        {
            Id = id;
        }
    }
}
