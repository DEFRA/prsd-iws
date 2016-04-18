namespace EA.Iws.Requests.NotificationMovements.Create
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Carriers;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanCreateExportMovements)]
    public class GetCarriers : IRequest<IList<CarrierData>>
    {
        public Guid NotificationId { get; private set; }

        public GetCarriers(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}