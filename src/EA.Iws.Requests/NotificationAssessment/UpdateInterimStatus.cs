namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(UserAdministrationPermissions.CanUpdateInterimStatus)]
    public class UpdateInterimStatus : IRequest<bool>
    {
        public UpdateInterimStatus(Guid notificationId, bool isInterim)
        {
            NotificationId = notificationId;
            IsInterim = isInterim;
        }

        public Guid NotificationId { get; private set; }

        public bool IsInterim { get; private set; }
    }
}
