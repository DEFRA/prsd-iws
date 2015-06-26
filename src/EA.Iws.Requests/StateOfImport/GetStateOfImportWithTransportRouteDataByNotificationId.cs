namespace EA.Iws.Requests.StateOfImport
{
    using System;
    using Prsd.Core.Mediator;

    public class GetStateOfImportWithTransportRouteDataByNotificationId : IRequest<StateOfImportWithTransportRouteData>
    {
        public Guid Id { get; private set; }

        public GetStateOfImportWithTransportRouteDataByNotificationId(Guid id)
        {
            Id = id;
        }
    }
}
