namespace EA.Iws.Requests.ImportNotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanChangeImportEntryExitPoint)]
    public class SetEntryPoint : IRequest<Unit>
    {
        public SetEntryPoint(Guid notificationId, Guid entryPointId)
        {
            NotificationId = notificationId;
            EntryPointId = entryPointId;
        }

        public Guid NotificationId { get; private set; }

        public Guid EntryPointId { get; private set; }
    }
}