namespace EA.Iws.Requests.NotificationMovements.Create
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.MeansOfTransport;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanCreateExportMovements)]
    public class GetMeansOfTransport : IRequest<IList<TransportMethod>>
    {
        public Guid NotificationId { get; private set; }

        public GetMeansOfTransport(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}