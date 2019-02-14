namespace EA.Iws.Requests.CustomsOffice
{
    using System;
    using Authorization;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class SetExitCustomsSelectionForNotificationById : IRequest<bool>
    {
        public Guid Id { get; private set; }
        public bool ExitSelection { get; private set; }

        public SetExitCustomsSelectionForNotificationById(Guid id, bool exitSelection)
        {
            Id = id;
            ExitSelection = exitSelection;
        }
    }
}