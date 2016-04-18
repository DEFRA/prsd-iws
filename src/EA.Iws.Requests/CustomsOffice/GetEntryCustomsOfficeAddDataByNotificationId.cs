namespace EA.Iws.Requests.CustomsOffice
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.CustomsOffice;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetEntryCustomsOfficeAddDataByNotificationId : IRequest<EntryCustomsOfficeAddData>
    {
        public Guid Id { get; private set; }

        public GetEntryCustomsOfficeAddDataByNotificationId(Guid id)
        {
            Id = id;
        }
    }
}
