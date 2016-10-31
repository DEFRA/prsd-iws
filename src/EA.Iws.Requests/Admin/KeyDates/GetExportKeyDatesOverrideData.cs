namespace EA.Iws.Requests.Admin.KeyDates
{
    using System;
    using Core.Admin.KeyDates;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(UserAdministrationPermissions.CanOverrideKeyDates)]
    public class GetExportKeyDatesOverrideData : IRequest<KeyDatesOverrideData>
    {
        public GetExportKeyDatesOverrideData(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}