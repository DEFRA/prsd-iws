namespace EA.Iws.Requests.ImportNotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(UserAdministrationPermissions.CanUpdateInterimStatus)]
    public class UpdateInterimStatus : IRequest<bool>
    {
        public UpdateInterimStatus(Guid importNotificationId, bool isInterim)
        {
            ImportNotificationId = importNotificationId;
            IsInterim = isInterim;
        }

        public Guid ImportNotificationId { get; private set; }

        public bool IsInterim { get; private set; }
    }
}
