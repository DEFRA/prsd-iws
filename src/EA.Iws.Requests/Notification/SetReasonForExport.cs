namespace EA.Iws.Requests.Notification
{
    using System;
    using Prsd.Core.Mediator;
    using Security;

    [NotificationReadOnlyAuthorize]
    public class SetReasonForExport : IRequest<string>
    {
        public SetReasonForExport(Guid notificationId, string reasonForExport)
        {
            NotificationId = notificationId;
            ReasonForExport = reasonForExport;
        }

        public Guid NotificationId { get; private set; }

        public string ReasonForExport { get; private set; }
    }
}