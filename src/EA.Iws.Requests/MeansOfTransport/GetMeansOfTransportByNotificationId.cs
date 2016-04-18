namespace EA.Iws.Requests.MeansOfTransport
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.MeansOfTransport;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetMeansOfTransportByNotificationId : IRequest<IList<TransportMethod>>
    {
        public Guid Id { get; private set; }

        public GetMeansOfTransportByNotificationId(Guid id)
        {
            Id = id;
        }
    }
}
