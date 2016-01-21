namespace EA.Iws.Requests.CustomsOffice
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetCustomsCompletionStatusByNotificationId : IRequest<CustomsOfficeCompletionStatus>
    {
        public Guid Id { get; private set; }

        public GetCustomsCompletionStatusByNotificationId(Guid id)
        {
            Id = id;
        }
    }
}
