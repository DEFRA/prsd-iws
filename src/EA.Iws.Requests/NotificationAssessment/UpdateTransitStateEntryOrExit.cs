namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanChangeTransitStateEntryExitPoint)]
    public class UpdateTransitStateEntryOrExit : IRequest<Guid>
    {
        public Guid NotificationId { get; private set; }

        public Guid TransitStateId { get; private set; }

        public Guid? EntryPointId { get; private set; }

        public Guid? ExitPointId { get; private set; }

        public UpdateTransitStateEntryOrExit(Guid notificationId, Guid transitStateId, Guid? entryPointId, Guid? exitPointId)
        {
            NotificationId = notificationId;
            TransitStateId = transitStateId;
            EntryPointId = entryPointId;
            ExitPointId = exitPointId;
        }
    }
}