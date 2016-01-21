namespace EA.Iws.Requests.MeansOfTransport
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.MeansOfTransport;
    using Prsd.Core.Mediator;
    using Security;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class SetMeansOfTransportForNotification : IRequest<Guid>
    {
        public Guid Id { get; private set; }

        public IList<MeansOfTransport> MeansOfTransport { get; private set; }

        public SetMeansOfTransportForNotification(Guid id, IList<MeansOfTransport> meansOfTransport)
        {
            Id = id;
            MeansOfTransport = meansOfTransport;
        }
    }
}
