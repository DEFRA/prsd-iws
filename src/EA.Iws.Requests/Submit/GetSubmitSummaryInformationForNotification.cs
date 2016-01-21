namespace EA.Iws.Requests.Submit
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Notification;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetSubmitSummaryInformationForNotification : IRequest<SubmitSummaryData>
    {
        public Guid Id { get; private set; }

        public GetSubmitSummaryInformationForNotification(Guid id)
        {
            Id = id;
        }
    }
}
