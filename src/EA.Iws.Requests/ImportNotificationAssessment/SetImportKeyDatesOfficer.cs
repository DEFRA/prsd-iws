namespace EA.Iws.Requests.ImportNotificationAssessment
{
    using System;
    using Core.Admin.KeyDates;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using EA.Iws.Core.ImportNotificationAssessment;
    using Prsd.Core.Mediator;

    [RequestAuthorization(UserAdministrationPermissions.CanOverrideKeyDates)]
    public class SetImportKeyDatesOfficer : IRequest<Unit>
    {
        public SetImportKeyDatesOfficer(Guid notificationId, string nameOfOfficer)
        {
            this.NotificationId = notificationId;
            this.NameOfOfficer = nameOfOfficer;
        }

        public Guid NotificationId { get; private set; }

        public string NameOfOfficer { get; private set; }
    }
}