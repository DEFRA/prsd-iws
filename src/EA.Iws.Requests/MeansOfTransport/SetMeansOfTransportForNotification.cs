namespace EA.Iws.Requests.MeansOfTransport
{
    using System;
    using System.Collections.Generic;
    using Authorization;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.MeansOfTransport;
    using Prsd.Core.Mediator;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class SetMeansOfTransportForNotification : IRequest<Guid>
    {
        public Guid Id { get; private set; }

        public IList<TransportMethod> MeansOfTransport { get; private set; }

        public SetMeansOfTransportForNotification(Guid id, IList<TransportMethod> meansOfTransport)
        {
            Id = id;
            MeansOfTransport = meansOfTransport;
        }
    }
}