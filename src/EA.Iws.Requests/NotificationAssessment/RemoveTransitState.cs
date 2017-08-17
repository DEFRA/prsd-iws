namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanAddRemoveTransitState)]
    public class RemoveTransitState : IRequest<Unit>
    {
        public RemoveTransitState(Guid notificationId, Guid transitStateId)
        {
            NotificationId = notificationId;
            TransitStateId = transitStateId;
        }

        public Guid NotificationId { get; private set; }

        public Guid TransitStateId { get; private set; }
    }
}