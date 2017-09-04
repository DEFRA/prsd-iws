namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Shared;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanEditContactDetails)]
    public class SetExporterContact : IRequest<Unit>
    {
        public SetExporterContact(Guid notificationId, ContactData contact)
        {
            NotificationId = notificationId;
            Contact = contact;
        }

        public Guid NotificationId { get; private set; }

        public ContactData Contact { get; private set; }
    }
}