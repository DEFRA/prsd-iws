namespace EA.Iws.Requests.CustomsOffice
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.CustomsOffice;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetExitCustomsOfficeAddDataByNotificationId : IRequest<ExitCustomsOfficeAddData>
    {
        public Guid Id { get; private set; }

        public GetExitCustomsOfficeAddDataByNotificationId(Guid id)
        {
            Id = id;
        }
    }
}
