namespace EA.Iws.Requests.CustomsOffice
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.CustomsOffice;
    using Domain.TransportRoute;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetEntryExitCustomsOfficeSelectionForNotificationById : IRequest<EntryExitCustomsOfficeSelectionData>
    {
        public Guid Id { get; private set; }

        public GetEntryExitCustomsOfficeSelectionForNotificationById(Guid id)
        {
            Id = id;
        }
    }
}
