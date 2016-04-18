namespace EA.Iws.Requests.Admin.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotificationAssessment)]
    public class SetCommencedDate : IRequest<bool>
    {
        public SetCommencedDate(Guid notificationId, DateTime commencementDate, string nameOfOfficer)
        {
            NotificationId = notificationId;
            CommencementDate = commencementDate;
            NameOfOfficer = nameOfOfficer;
        }

        public Guid NotificationId { get; private set; }

        public DateTime CommencementDate { get; private set; }

        public string NameOfOfficer { get; private set; }
    }
}