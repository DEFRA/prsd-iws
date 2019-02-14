namespace EA.Iws.Requests.CustomsOffice
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.CustomsOffice;
    using Domain.TransportRoute;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetEntryExitCustomsSelectionForNotificationById : IRequest<EntryExitCustomsSelectionData>
    {
        public Guid Id { get; private set; }

        public GetEntryExitCustomsSelectionForNotificationById(Guid id)
        {
            Id = id;
        }
    }
}
