namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanMakeExportNotificationAssessmentDecision)]
    public class ObjectNotificationApplication : IRequest<bool>
    {
        public Guid Id { get; private set; }

        public string Reason { get; private set; }

        public DateTime Date { get; private set; }

        public ObjectNotificationApplication(Guid id, DateTime date, string reason)
        {
            Id = id;
            Date = date;
            Reason = reason;
        }
    }
}