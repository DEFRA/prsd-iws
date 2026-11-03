namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(UserAdministrationPermissions.CanOverrideKeyDates)]
    public class SetKeyDatesOfficer : IRequest<Unit>
    {
        public SetKeyDatesOfficer(Guid notificationId, string nameOfOfficer)
        {
            this.NotificationId = notificationId;
            this.NameOfOfficer = nameOfOfficer;
        }

        public Guid NotificationId { get; private set; }

        public string NameOfOfficer { get; private set; }
    }
}
